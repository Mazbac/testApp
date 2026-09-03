import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { spawnSync } from 'node:child_process';
import { root } from './contract.mjs';

function run(dir, script = 'validate.mjs') {
  return spawnSync(process.execPath, [path.join(dir, '.automation', script)], { cwd: dir, encoding: 'utf8' });
}

function copyCase(name) {
  const dir = fs.mkdtempSync(path.join(os.tmpdir(), `newProject-${name}-`));
  fs.cpSync(root, dir, {
    recursive: true,
    filter: (source) => path.basename(source) !== '.git' && path.basename(source) !== 'node_modules'
  });
  return dir;
}

function expectFailure(name, mutate, expected, script = 'validate.mjs') {
  const dir = copyCase(name);
  try {
    mutate(dir);
    const result = run(dir, script);
    const output = `${result.stdout}\n${result.stderr}`;
    if (result.status === 0 || !output.includes(expected)) {
      throw new Error(`${name} did not fail as expected. Output:\n${output}`);
    }
    console.log(`OK negative canary: ${name}`);
  } finally {
    fs.rmSync(dir, { recursive: true, force: true });
  }
}

const baseline = run(root);
if (baseline.status !== 0) {
  console.error(baseline.stdout, baseline.stderr);
  throw new Error('baseline validation must pass before negative canaries');
}
console.log('OK positive canary: baseline');

expectFailure('schema-version', (dir) => {
  const file = path.join(dir, '.project', 'manifest.json');
  const manifest = JSON.parse(fs.readFileSync(file, 'utf8'));
  manifest.schemaVersion = 999;
  fs.writeFileSync(file, `${JSON.stringify(manifest, null, 2)}\n`);
}, '$.schemaVersion must equal 2');

expectFailure('unpinned-action', (dir) => {
  const file = path.join(dir, '.github', 'workflows', 'template-integrity.yml');
  const text = fs.readFileSync(file, 'utf8').replace(/actions\/checkout@[0-9a-f]{40}/i, 'actions/checkout@v6');
  fs.writeFileSync(file, text);
}, 'action is not pinned to a full commit SHA');

const baselineLifecycle = JSON.parse(fs.readFileSync(path.join(root, '.project', 'manifest.json'), 'utf8')).lifecycle;
expectFailure('status-drift', (dir) => {
  const file = path.join(dir, 'STATUS.md');
  const wrongLifecycle = baselineLifecycle === 'BUILD' ? 'VERIFY' : 'BUILD';
  const text = fs.readFileSync(file, 'utf8').replace(/- Lifecycle: [A-Z_]+/, `- Lifecycle: ${wrongLifecycle}`);
  fs.writeFileSync(file, text);
}, `STATUS.md lifecycle must match manifest lifecycle ${baselineLifecycle}`, 'context-integrity.mjs');

expectFailure('missing-ui-design-source', (dir) => {
  const manifestFile = path.join(dir, '.project', 'manifest.json');
  const manifest = JSON.parse(fs.readFileSync(manifestFile, 'utf8'));
  manifest.mode = 'project';
  manifest.lifecycle = 'DESIGN';
  manifest.project = { name: 'Canary', summary: 'Canary project', appType: 'desktop', primaryUsers: ['tester'], firstRelease: 'First value' };
  manifest.stack = { primary: 'JavaScript', runtime: 'Node', packageManager: 'npm', database: 'none' };
  manifest.commands.verify = 'node .automation/validate.mjs';
  manifest.ui = {
    hasUserInterface: true,
    kind: 'desktop',
    platform: 'windows',
    devUrl: null,
    playwright: false,
    designSystem: 'canary/missing-design-system.md',
    designTokens: 'src/design-tokens.json',
    componentStrategy: 'product-components-over-proven-primitives',
    accessibilityTarget: 'platform accessibility requirements',
    visualRegression: 'required-when-stable'
  };
  manifest.experience = {
    audience: 'consumer',
    installer: 'native-installer',
    onboarding: 'contextual',
    updater: 'not-applicable',
    recovery: 'required',
    reset: 'not-applicable',
    uninstaller: 'required',
    goldenJourneys: ['new user reaches first value']
  };
  manifest.release = { artifact: 'desktop-package', deployment: 'local-install', rollback: 'previous-version-install' };
  fs.writeFileSync(manifestFile, `${JSON.stringify(manifest, null, 2)}\n`);
  fs.writeFileSync(path.join(dir, 'PROJECT.md'), '# Project Definition\n\nConcrete canary project.\n');
  fs.writeFileSync(path.join(dir, 'ARCHITECTURE.md'), '# Architecture Record\n\nConcrete canary architecture.\n');
  fs.writeFileSync(path.join(dir, 'STATUS.md'), '# Current State\n\n- Lifecycle: DESIGN\n');
  fs.writeFileSync(path.join(dir, '.github', 'workflows', 'quality.yml'), 'name: Quality\non: [push]\njobs: {}\n');
}, 'UI design source does not exist: canary/missing-design-system.md', 'context-integrity.mjs');


expectFailure('unsupported-schema-keyword', (dir) => {
  const file = path.join(dir, '.project', 'manifest.schema.json');
  const schema = JSON.parse(fs.readFileSync(file, 'utf8'));
  schema.properties.project.properties.name.pattern = '^x$';
  fs.writeFileSync(file, `${JSON.stringify(schema, null, 2)}\n`);
}, 'uses unsupported schema keyword: pattern');

expectFailure('missing-quality-context-gate', (dir) => {
  const manifestFile = path.join(dir, '.project', 'manifest.json');
  const manifest = JSON.parse(fs.readFileSync(manifestFile, 'utf8'));
  manifest.mode = 'project';
  manifest.lifecycle = 'DEFINE';
  manifest.project = { name: 'Canary', summary: 'Canary project', appType: 'service', primaryUsers: ['tester'], firstRelease: 'First value' };
  manifest.stack = { primary: 'JavaScript', runtime: 'Node', packageManager: 'npm', database: 'none' };
  manifest.commands.verify = 'node .automation/validate.mjs';
  manifest.ui = { hasUserInterface: false, kind: 'none', platform: 'none', devUrl: null, playwright: false, designSystem: null, designTokens: null, componentStrategy: 'not-applicable', accessibilityTarget: 'not-applicable', visualRegression: 'not-applicable' };
  manifest.experience = { audience: 'technical-user', installer: 'not-applicable', onboarding: 'not-applicable', updater: 'not-applicable', recovery: 'documented', reset: 'not-applicable', uninstaller: 'not-applicable', goldenJourneys: [] };
  manifest.release = { artifact: 'source', deployment: 'not-applicable', rollback: 'git-revert' };
  fs.writeFileSync(manifestFile, `${JSON.stringify(manifest, null, 2)}\n`);
  fs.writeFileSync(path.join(dir, 'PROJECT.md'), '# Project Definition\n\nConcrete canary project.\n');
  fs.writeFileSync(path.join(dir, 'ARCHITECTURE.md'), '# Architecture Record\n\nConcrete canary architecture.\n');
  fs.writeFileSync(path.join(dir, 'STATUS.md'), '# Current State\n\n- Lifecycle: DEFINE\n');
  fs.writeFileSync(path.join(dir, '.github', 'workflows', 'quality.yml'), 'name: Quality\non: [push]\njobs:\n  quality:\n    runs-on: ubuntu-latest\n    steps:\n      - run: node .automation/validate.mjs\n');
}, 'project quality workflow must run context integrity', 'context-integrity.mjs');

console.log('Self-test OK');
