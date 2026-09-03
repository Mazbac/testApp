using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuickShelf.Models;

public sealed class Snippet : INotifyPropertyChanged
{
    private string _title = "Untitled snippet";
    private string _content = string.Empty;
    private bool _isFavorite;
    private DateTimeOffset _updatedAtUtc = DateTimeOffset.UtcNow;

    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public string Title { get => _title; set => SetField(ref _title, value); }
    public string Content { get => _content; set => SetField(ref _content, value); }
    public bool IsFavorite { get => _isFavorite; set => SetField(ref _isFavorite, value); }
    public DateTimeOffset UpdatedAtUtc { get => _updatedAtUtc; set => SetField(ref _updatedAtUtc, value); }

    public event PropertyChangedEventHandler? PropertyChanged;

    public Snippet Clone() => new()
    {
        Id = Id,
        Title = Title,
        Content = Content,
        IsFavorite = IsFavorite,
        CreatedAtUtc = CreatedAtUtc,
        UpdatedAtUtc = UpdatedAtUtc
    };

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
