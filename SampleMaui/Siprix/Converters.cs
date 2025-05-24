using System.Globalization;

namespace SampleMaui;

/// [MicMute IconConverter] ////////////////////////////////////////////////////////////////

public class MicMuteIconConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool micMuted) return Binding.DoNothing;
        else return micMuted ? Icons.mic_off : Icons.mic;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

/// [CamMute IconConverter] ////////////////////////////////////////////////////////////////

public class CamMuteIconConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool camMuted) return Binding.DoNothing;
        else return camMuted ? Icons.videocam_off : Icons.videocam;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

/// [Mute TextConverter] ////////////////////////////////////////////////////////////////

public class MuteTextConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool muted) return Binding.DoNothing;
        else return muted ? "UnMute" : "Mute";
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

/// [PlayTextConverter IconConverter] ////////////////////////////////////////////////////////////////
public class PlayTextConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool isFilePlaying) return Binding.DoNothing;
        else return isFilePlaying ? "Stop\nplay" : "Start\nplay";
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

/// [RecordTextConverter IconConverter] ////////////////////////////////////////////////////////////////
public class RecordTextConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool isFileRec) return Binding.DoNothing;
        else return isFileRec ? "Stop\nrecord" : "Start\nrecord";
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

/// [CallDirection IconConverter] ////////////////////////////////////////////////////////////////
public class CallDirectionIconConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if(value is not bool isIncoming) return Binding.DoNothing;
        else return isIncoming ? Icons.call_received : Icons.call_made;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}


/// [HoldState IconConverter] ////////////////////////////////////////////////////////////////

public class HoldStateIconConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Siprix.HoldState holdState) return Binding.DoNothing;
        return (holdState == Siprix.HoldState.Local) ||
               (holdState == Siprix.HoldState.LocalAndRemote) ? Icons.play_arrow : Icons.pause;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

/// [HoldState TextConverter] ////////////////////////////////////////////////////////////////

public class HoldStateTextConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Siprix.HoldState holdState) return Binding.DoNothing;
        return (holdState == Siprix.HoldState.Local) ||
               (holdState == Siprix.HoldState.LocalAndRemote) ? "Unhold" : "Hold";
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}


/// [RegState IconConverter] ////////////////////////////////////////////////////////////////

public class RegStateIconConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Siprix.RegState regState) return Binding.DoNothing;

        switch (regState)
        {
            case Siprix.RegState.Success:    return Icons.cloud_done;
            case Siprix.RegState.Failed:     return Icons.cloud_off;
            case Siprix.RegState.InProgress: return Icons.sync;
            default:                         return Icons.done;
        }
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return Siprix.RegState.Success;
    }
}


public class StateColorConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool state) return Binding.DoNothing;
        return state ? Colors.Red : Colors.White;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return Siprix.RegState.Success;
    }
}

/// [RegState ColorConverter] ////////////////////////////////////////////////////////////////

public class RegStateColorConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Siprix.RegState regState) return Binding.DoNothing;

        switch (regState)
        {
            case Siprix.RegState.Success:    return Colors.Green;
            case Siprix.RegState.Failed:     return Colors.Red;
            case Siprix.RegState.InProgress: return Colors.Blue;
            default:                         return Colors.Gray;
        }
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return Siprix.RegState.Success;
    }
}

/// [MessageSentColorConverter ColorConverter] ////////////////////////////////////////////////////////////////

public class MessageSentColorConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool sentSuccess) return Binding.DoNothing;
        else return sentSuccess ? Colors.BlueViolet : Colors.Gray;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}    


/// [BLFState ColorConverter] ////////////////////////////////////////////////////////////////

public class BLFStateColorConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Siprix.BLFState blfState) return Binding.DoNothing;

        switch (blfState)
        {
            case Siprix.BLFState.SubscriptionDestroyed: return new SolidColorBrush(Colors.Gray);
            case Siprix.BLFState.Terminated:
            case Siprix.BLFState.Unknown: return new SolidColorBrush(Colors.Green); //Ready to make call
            default: return new SolidColorBrush(Colors.Red); //Call in progress
        }
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return Siprix.BLFState.Unknown;
    }
}


/// [CdrState IconConverter] ////////////////////////////////////////////////////////////////

public class CdrStateIconConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Siprix.CdrState cdrState) return Binding.DoNothing;

        switch (cdrState)
        {
            case Siprix.CdrState.IncomingConnected: return Icons.call_received;
            case Siprix.CdrState.IncomingMissed: return Icons.call_missed;
            case Siprix.CdrState.OutgoingConnected: return Icons.call_made;
            default: return Icons.call_missed_outgoing;
        }
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

public class CdrStateColorConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Siprix.CdrState cdrState) return Binding.DoNothing;

        switch (cdrState)
        {
            case Siprix.CdrState.IncomingConnected: return Colors.Green;
            case Siprix.CdrState.IncomingMissed: return Colors.Red;
            case Siprix.CdrState.OutgoingConnected: return Colors.LightGreen;
            default: return Colors.Orange;
        }
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

/// [SwitchedCall ColorConverter] ////////////////////////////////////////////////////////////////

public class SwitchedCallColorConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool isSwitchedCall) return Binding.DoNothing;
        else return isSwitchedCall ? Colors.LightSkyBlue : Colors.Transparent;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}

/// [BooleanToVisibilityConverter] ////////////////////////////////////////////////////////////////

public class InverseBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool isVisible) return Binding.DoNothing;
        return !isVisible;
    }
    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return false;
    }
}
