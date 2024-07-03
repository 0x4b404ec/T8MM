using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using T8MM.ViewModels;

namespace T8MM;

public class ViewLocator : IDataTemplate
{
    private readonly Dictionary<object, Control> m_controls = new();


    public Control? Build(object? data)
    {
        var fullName = data?.GetType().FullName;
        if (fullName is null)
            return new TextBlock { Text = "Data is null or has no name." };

        var name = fullName.Replace("ViewModel", "View");
        var type = Type.GetType(name);
        if (type is null)
            return new TextBlock { Text = $"$No view for {name}." };

        if (!m_controls.TryGetValue(data!, out var res))
        {
            res ??= (Control)Activator.CreateInstance(type)!;
            m_controls[data!] = res;
        }

        res.DataContext = data;
        return res;
    }
    
    // public Control? Build(object? data)
    // {
    //     if (data is null)
    //         return null;
    //
    //     var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
    //     var type = Type.GetType(name);
    //
    //     if (type != null)
    //     {
    //         var control = (Control)Activator.CreateInstance(type)!;
    //         control.DataContext = data;
    //         return control;
    //     }
    //
    //     return new TextBlock { Text = "Not Found: " + name };
    // }

    public bool Match(object? data)
    {
        return data is INotifyPropertyChanged;
    }
}