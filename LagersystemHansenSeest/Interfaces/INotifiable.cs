using System;
using System.Collections.Generic;
using System.Text;

namespace LagersystemHansenSeest.Interfaces;

// Interface to generate notifications
public interface INotifiable
{
    bool SetNotification();

    string GetNotification();
}
