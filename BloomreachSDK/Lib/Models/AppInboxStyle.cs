
namespace Bloomreach;

public class AppInboxStyle
{
    public ButtonStyle? AppInboxButton { get; set; }
    public DetailViewStyle? DetailView { get; set; }
    public ListScreenStyle? ListView { get; set; }
}

public class ListScreenStyle
{
    public TextViewStyle? EmptyTitle { get; set; }
    public TextViewStyle? EmptyMessage { get; set; }
    public TextViewStyle? ErrorTitle { get; set; }
    public TextViewStyle? ErrorMessage { get; set; }
    public ProgressBarStyle? Progress { get; set; }
    public AppInboxListViewStyle? List { get; set; }
}

public class AppInboxListViewStyle
{
    public string? BackgroundColor { get; set; }
    public AppInboxListItemStyle? Item { get; set; }
}

public class AppInboxListItemStyle
{
    public string? BackgroundColor { get; set; }
    public ImageViewStyle? ReadFlag { get; set; }
    public TextViewStyle? ReceivedTime { get; set; }
    public TextViewStyle? Title { get; set; }
    public TextViewStyle? Content { get; set; }
    public ImageViewStyle? Image { get; set; }
}

public class ProgressBarStyle
{
    public bool? Visible { get; set; }
    public string? ProgressColor { get; set; }
    public string? BackgroundColor { get; set; }
}

public class DetailViewStyle
{
    public TextViewStyle? Title { get; set; }
    public TextViewStyle? Content { get; set; }
    public TextViewStyle? ReceivedTime { get; set; }
    public ImageViewStyle? Image { get; set; }
    public ButtonStyle? Button { get; set; }
}

public class ImageViewStyle
{
    public bool? Visible { get; set; }
    public string? BackgroundColor { get; set; }
}

public class TextViewStyle
{
    public bool? Visible { get; set; }
    public string? TextColor { get; set; }
    public String? TextSize { get; set; }
    public String? TextWeight { get; set; }
    public String? TextOverride { get; set; }
}

public class ButtonStyle
{
    public string? TextOverride { get; set; }
    public string? TextColor { get; set; }
    public string? BackgroundColor { get; set; }
    public string? ShowIcon { get; set; }
    public string? TextSize { get; set; }
    public bool? Enabled { get; set; }
    public string? BorderRadius { get; set; }
    public string? TextWeight { get; set; }
}