using Android.Views;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using AndroidButton = Android.Widget.Button;
using AView = Android.Views.View;
using Color = Android.Graphics.Color;
using LayoutAlignment = Microsoft.Maui.Primitives.LayoutAlignment;

namespace Bloomreach.Platforms.Android;

/**
 * Specialized ViewHandler that creates Button instance from BloomreachSDK for AppInbox feature (see method `CreatePlatformView`).
 * This handler is "pure" copy of https://github.com/dotnet/maui/blob/main/src/Core/src/Handlers/Button/ButtonHandler.Android.cs
 * except that MAUI operates with MaterialButton and Bloomreach SDK returns basic Android.Widget.Button;
 */
public class AppInboxButtonAndroidHandler : ViewHandler<IButton, AndroidButton>
{
    
    public static readonly Thickness DefaultPadding = new Thickness(16, 8.5);
    
    public static IPropertyMapper<ITextButton, AppInboxButtonAndroidHandler> TextButtonMapper
        = new PropertyMapper<ITextButton, AppInboxButtonAndroidHandler>()
    {
        [nameof(ITextStyle.CharacterSpacing)] = MapCharacterSpacing,
        [nameof(ITextStyle.Font)] = MapFont,
        [nameof(ITextStyle.TextColor)] = MapTextColor,
        // [nameof(IText.Text)] = MapText
    };

    public static IPropertyMapper<IButton, AppInboxButtonAndroidHandler> Mapper
        = new PropertyMapper<IButton, AppInboxButtonAndroidHandler>(TextButtonMapper, ViewMapper)
    {
        [nameof(IButton.Background)] = MapBackground,
        [nameof(IButton.Padding)] = MapPadding,
        [nameof(IButtonStroke.StrokeThickness)] = MapStrokeThickness,
        [nameof(IButtonStroke.StrokeColor)] = MapStrokeColor,
        [nameof(IButtonStroke.CornerRadius)] = MapCornerRadius
    };

    public static CommandMapper<IButton, AppInboxButtonAndroidHandler> CommandMapper = new(ViewCommandMapper);
    
    public AppInboxButtonAndroidHandler() : base(Mapper, CommandMapper) { }

    public AppInboxButtonAndroidHandler(IPropertyMapper? mapper) : base(mapper ?? Mapper, CommandMapper) { }

    public AppInboxButtonAndroidHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper) { }

    /**
     * Produces AppInboxButton from SDK always.
     */
    protected override AndroidButton CreatePlatformView()
    {
        var appInboxNativeButton = BloomreachSDK.GetAppInboxNativeButton() as AndroidButton ?? CreateFallbackButtonInstance();
        appInboxNativeButton.SoundEffectsEnabled = false;
        return appInboxNativeButton;
    }

    public override void PlatformArrange(Rect frame)
    {
        PrepareForTextViewArrange(frame);
        base.PlatformArrange(frame);
    }

    protected override void ConnectHandler(AndroidButton platformView)
    {
        platformView.FocusChange += OnNativeViewFocusChange;
        platformView.LayoutChange += OnPlatformViewLayoutChange;

        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(AndroidButton platformView)
    {
        platformView.FocusChange -= OnNativeViewFocusChange;
        platformView.LayoutChange -= OnPlatformViewLayoutChange;

        base.DisconnectHandler(platformView);
    }
    
    private void OnNativeViewFocusChange(object? sender, AView.FocusChangeEventArgs e)
    {
        if (VirtualView != null)
        {
            VirtualView.IsFocused = e.HasFocus;
        }
    }
    
    void OnPlatformViewLayoutChange(object? sender, AView.LayoutChangeEventArgs e)
    {
        if (sender is AndroidButton platformView && VirtualView != null)
        {
            platformView.UpdateBackground(VirtualView);
        }
    }

    private void PrepareForTextViewArrange(Rect frame)
    {
        if (frame.Width < 0 || frame.Height < 0)
        {
            return;
        }

        var platformView = PlatformView;
        if (platformView == null)
        {
            return;
        }
        var virtualView = VirtualView;
        if (virtualView == null)
        {
            return;
        }
        if (NeedsExactMeasure(virtualView))
        {
            platformView.Measure(
                MakeMeasureSpecExact(platformView, frame.Width),
                MakeMeasureSpecExact(platformView, frame.Height)
            );
        }
    }

    private int MakeMeasureSpecExact(AndroidButton view, double size)
    {
        // Convert to a native size to create the spec for measuring
        var deviceSize = ToPixels(view, size);
        return MeasureSpecMode.Exactly.MakeMeasureSpec(deviceSize);
    }

    private int ToPixels(AView view, double size)
    {
        var context = view.Context ?? Platform.AppContext;
        using var metrics = context.Resources?.DisplayMetrics;
        var displayDensity = metrics?.Density ?? 1;
        return (int)Math.Ceiling(size * displayDensity);
    }

    private bool NeedsExactMeasure(IButton virtualView)
    {
        if (virtualView.VerticalLayoutAlignment != LayoutAlignment.Fill
            && virtualView.HorizontalLayoutAlignment != LayoutAlignment.Fill)
        {
            // Layout Alignments of Start, Center, and End will be laying out the TextView at its measured size,
            // so we won't need another pass with MeasureSpecMode.Exactly
            return false;
        }
        if (virtualView is { Width: >= 0, Height: >= 0 })
        {
            // If the Width and Height are both explicit, then we've already done MeasureSpecMode.Exactly in 
            // both dimensions; no need to do it again
            return false;
        }
        // We're going to need a second measurement pass so TextView can properly handle alignments
        return true;
    }

    private static AndroidButton CreateFallbackButtonInstance()
    {
        return new AndroidButton(Platform.CurrentActivity)
        {
            Text = "AppInbox not ready"
        };
    }

    private static void MapText(AppInboxButtonAndroidHandler handler, ITextButton button)
    {
        handler.PlatformView?.UpdateTextPlainText(button);
    }

    private static void MapTextColor(AppInboxButtonAndroidHandler handler, ITextButton button)
    {
        handler.PlatformView?.UpdateTextColor(button);
    }

    private static void MapFont(AppInboxButtonAndroidHandler handler, ITextButton button)
    {
        var context = handler.MauiContext ??
                      throw new InvalidOperationException($"Unable to find the context. The {nameof(ElementHandler.MauiContext)} property should have been set by the host.");
        var services = context?.Services ??
                       throw new InvalidOperationException($"Unable to find the service provider. The {nameof(ElementHandler.MauiContext)} property should have been set by the host.");
        var fontManager = services.GetRequiredService<IFontManager>();
        handler.PlatformView?.UpdateFont(button, fontManager);
    }

    private static void MapCharacterSpacing(AppInboxButtonAndroidHandler handler, ITextButton button)
    {
        handler.PlatformView?.UpdateCharacterSpacing(button);
    }
    
    private static void MapCornerRadius(AppInboxButtonAndroidHandler handler, IButton button)
    {
        var platformView = handler.PlatformView;
        if (platformView.Background is BorderDrawable)
        {
            UpdateBorderDrawable(platformView, button);
        }
    }

    private static void MapStrokeColor(AppInboxButtonAndroidHandler handler, IButton button)
    {
        var platformView = handler.PlatformView;
        if (platformView.Background is BorderDrawable)
        {
            UpdateBorderDrawable(platformView, button);
        }
    }
    
    private static void UpdateBorderDrawable(AndroidButton platformView, IButton button)
    {
        var background = button.Background;
        if (background.IsNullOrEmpty()) return;
        // Remove previous background gradient if any
        if (platformView.Background is BorderDrawable previousBackground)
        {
            platformView.Background = null;
            previousBackground.Dispose();
        }
        var mauiDrawable = new BorderDrawable(platformView.Context);
        platformView.BackgroundTintList = null;
        platformView.Background = mauiDrawable;
        mauiDrawable.SetBackground(background);
        mauiDrawable.SetBorderBrush(new SolidPaint { Color = button.StrokeColor });
        if (button.StrokeThickness > 0)
        {
            mauiDrawable.SetBorderWidth(button.StrokeThickness);
        }
        if (button.CornerRadius > 0)
        {
            mauiDrawable.SetCornerRadius(button.CornerRadius);
        }
        else
        {
            const int defaultCornerRadius = 2; // Default value for Android material button.
            mauiDrawable.SetCornerRadius(platformView.Context.ToPixels(defaultCornerRadius));
        }
    }

    private static void MapStrokeThickness(AppInboxButtonAndroidHandler handler, IButton button)
    {
        var platformView = handler.PlatformView;
        if (platformView.Background is BorderDrawable)
        {
            UpdateBorderDrawable(platformView, button);
            return;
        }
        if (button is not IButtonStroke { StrokeThickness: >= 0 } buttonStroke) return;
        {
            if (platformView.Background is not MauiDrawable mauiDrawable)
            {
                return;
            }
            mauiDrawable.SetBorderWidth(button.StrokeThickness);
        }
    }

    private static void MapPadding(AppInboxButtonAndroidHandler handler, IButton button)
    {
        handler.PlatformView?.UpdatePadding(button, DefaultPadding);
    }
    
}