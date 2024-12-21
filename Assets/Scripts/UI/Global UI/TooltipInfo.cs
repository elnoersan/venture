using System;
using UnityEngine;

[Serializable]
public class TooltipInfo
{
    [SerializeField] private string title; // The title of the tooltip
    [SerializeField] private string subtitle; // The subtitle of the tooltip
    [SerializeField][TextArea] private string body; // The body text of the tooltip, using a TextArea for better editing in the Inspector

    // Constructor to initialize the TooltipInfo with title, subtitle, and body
    public TooltipInfo(string title, string subtitle, string body)
    {
        this.title = title;
        this.subtitle = subtitle;
        this.body = body;
    }

    // Property for accessing or modifying the title
    public string Title
    {
        get => title;
        set => title = value;
    }

    // Property for accessing or modifying the subtitle
    public string Subtitle
    {
        get => subtitle;
        set => subtitle = value;
    }

    // Property for accessing or modifying the body text
    public string Body
    {
        get => body;
        set => body = value;
    }
}