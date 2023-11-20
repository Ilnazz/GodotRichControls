﻿using CommunityToolkit.Mvvm.ComponentModel;
using Godot;

[GlobalClass]
[INotifyPropertyChanged]
public partial class RichTextEdit : TextEdit
{
    public new string Text
    {
        get => base.Text;
        set
        {
            if (base.Text == value)
                return;

            SetBlockSignals(true);
            base.Text = value;
            SetBlockSignals(false);

            OnPropertyChanged();
        }
    }

    public RichTextEdit() => TextChanged += () => OnPropertyChanged(nameof(Text));
}