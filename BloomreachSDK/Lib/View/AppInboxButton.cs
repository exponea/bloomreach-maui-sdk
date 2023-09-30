using System;
using System.Linq;
using Microsoft.Maui.Controls;
#if IOS
using UIKit;
#endif

namespace Bloomreach.View;

public class AppInboxButton : Button
{
#if IOS
    private const string AppInboxIconBase64 = "iVBORw0KGgoAAAANSUhEUgAAABcAAAAYCAQAAAC7da7+AAAAIGNIUk0AAHomAACAhAAA+gAAAIDoAAB1MAAA6mAAADqYAAAXcJy6UTwAAAACYktHRAD/h4/MvwAAAAlwSFlzAAAhOAAAITgBRZYxYAAAAAd0SU1FB+cKAgovH7Wv4f4AAAGKSURBVDjLvdM7aFRREIDh72aTiPGJXoNggoqgoKgpLMQihZIFq2gRAmIhQbCyCNqonYUEu4gIIdoIFiJoI4iKnYgaFkVQA2kSFQWxkMQ81t27x8K7D8MupspMMzPnP/M6HJZf1tgus3T8vA+66h001cW32SFeCr7WepGgJMjYYGVjPHbZY7dtFFB02DMP9NcyVXOVIZe0mlRII9Om7DHiRL22BiXu2qLHTsPmZXXb65BJUw4sho/47r1d2jz1wj3TbvjmHAbkPbGpFt7qlVl94KA3fimYdUuMFUYEQ5rLcMaw4Gplji45iVHrUr/TmBlHoRmxrKIOVzQhMuaU3XLOpLsvKVot61H59ifBfKrBW+3olZeksbzgZjk7ka8G/RDhrF7X5JwUueC1jESPi9VBO30xoT319nkpCOZc15bG+v/NTlSx3unTLfbRc3OVZaRSxZNKtc/uLHqVUIsXLdhswLhIfTmGhbLTYlT4j844/rcJ6HDafq0Ncgc/PXTfbzXlWxp8FSgoWQb5A4Mkfc2swvuGAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDIzLTEwLTAyVDEwOjQ3OjIzKzAwOjAwa+RB+gAAACV0RVh0ZGF0ZTptb2RpZnkAMjAyMy0xMC0wMlQxMDo0NzoyMyswMDowMBq5+UYAAAAodEVYdGRhdGU6dGltZXN0YW1wADIwMjMtMTAtMDJUMTA6NDc6MzErMDA6MDAWmckuAAAAAElFTkSuQmCC";
#else
    private const string AppInboxIconBase64 = "iVBORw0KGgoAAAANSUhEUgAAAEUAAABICAMAAACXzcjFAAAABGdBTUEAALGPC/xhBQAAAAFzUkdCAK7OHOkAAAAJcEhZcwAAITgAACE4AUWWMWAAAAA/UExURUdwTAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALoT4l0AAAAUdFJOUwDvMHAgvxDfn0Bgz4BQr5B/j7CgEZfI5wAAAcJJREFUWMPtl9mWhCAMRBtFNlF74f+/dZBwRoNBQftpxnrpPqjXUFRQH49bt74o0ejrEC6dk+IqRTkve5XSzRR1U/4oRRirlHphih9RduDFcQ1J85HFFBh0qozTMLdHcZLXQHKUEszcfQeUAgxAWMu9MMUPNC0U2h1AnunNkpWOpT53IQNUAhCR1AK2waT0sSltdLkXC4V3scIWqhUHQY319/6fx0RK4L9XJ41lpnRg42f+mUS/mMrZUjAhjaYMuXnZcMW4siua55o9UyyOX+iGUJf8vWzasWZcopamOFl9Afd7ZU1hnM4xzmtc7q01nDqwYJLQt9tbrpKvMl216ZyO7ASTaTPAEOOMijBYa+iV64gehjlNeDCkyvUKK1B1WGGTHOqpKTlaGrfpRrKIYvAETlIm9OpwlsIESlMRha3tg0T0YhX5cX08S0FjIt7NaN1K4pIySuzclewZipDHHhSMNTKzM1RR0M6w6YJiis99FxnbJ0cFxbujB9NQe2MVJat/RHEVXw2corCad7Z56eLmSO3pHl6oePobU6w7pWS7F+wMRNJPhkoNG7/q58QMtXYfWTUbK/Lfq4Xilz9Ib926qB8ZxV6DpmAIowAAAABJRU5ErkJggg==";
#endif
    
    public AppInboxButton() : base()
    {
        MapNativeButtonClick();
        ApplyButtonStyle();
    }

    private void ApplyButtonStyle()
    {
        var style = BloomreachSDK.Instance.AppInboxButtonStyle;
        if (style.BackgroundColor != null)
        {
            if (Color.TryParse(style.BackgroundColor, out var parsedColor))
            {
                this.BackgroundColor = parsedColor;
            }
            else
            {
                BloomreachSDK.ThrowOrLog(BloomreachException.Common(
                    $"Unable to parse color {style.BackgroundColor} for App Inbox"
                ));
                this.BackgroundColor = Color.FromArgb("#FF3F06CA");
            }
        }
        this.IsEnabled = style.Enabled ?? true;
        if (style.TextColor != null)
        {
            this.TextColor = Color.Parse(style.TextColor);
        }
        this.Text = style.TextOverride ?? "Inbox";
        if (style.TextWeight != null)
        {
            if (style.TextWeight.ToLower() == "bold")
            {
                this.FontAttributes = FontAttributes.Bold;
            }
            else if (style.TextWeight.ToLower() == "italic")
            {
                this.FontAttributes = FontAttributes.Italic;
            }
            else
            {
                this.FontAttributes = FontAttributes.None;
            }
        }
        if (style.BorderRadius != null)
        {
            var sizeIndependent = SizeIndependent(style.BorderRadius);
            if (sizeIndependent.Length != 0)
            {
                this.CornerRadius = Convert.ToInt32(sizeIndependent);
            }
            else
            {
                this.CornerRadius = 8;
            }
        }
        if (style.TextSize != null)
        {
            var sizeIndependent = SizeIndependent(style.TextSize);
            if (sizeIndependent.Length != 0)
            {
                this.FontSize = Convert.ToInt32(sizeIndependent);
            }
            else
            {
                this.FontSize = 14;
            }
        }
        if (style.ShowIcon == true)
        {
            var imageBytes = Convert.FromBase64String(AppInboxIconBase64);
            var imageDecodeStream = new MemoryStream(imageBytes);
            var imageSource = ImageSource.FromStream(() => imageDecodeStream);
            this.ImageSource = imageSource;
        }
    }

    private static string SizeIndependent(string source)
    {
        return source.Substring(
            0,
            source
                .ToCharArray()
                .TakeWhile(char.IsDigit)
                .Count()
        );
    }

    private void MapNativeButtonClick()
    {
        var nativeButton = BloomreachSDK.GetAppInboxNativeButton();
        if (nativeButton == null)
        {
            BloomreachSDK.ThrowOrLog(BloomreachException.Common("App Inbox button is not ready yet"));
            return;
        }
        this.Clicked += delegate
        {
#if IOS
            (nativeButton as UIButton)?.SendActionForControlEvents(UIControlEvent.TouchUpInside);
#elif ANDROID
            (nativeButton as Android.Widget.Button)?.PerformClick();
#endif
        };
    }
}