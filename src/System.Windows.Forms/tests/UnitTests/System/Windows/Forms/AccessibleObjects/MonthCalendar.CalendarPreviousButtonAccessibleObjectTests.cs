﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using static System.Windows.Forms.MonthCalendar;

namespace System.Windows.Forms.Tests.AccessibleObjects;

public class MonthCalendar_CalendarPreviousButtonAccessibleObjectTests
{
    [WinFormsFact]
    public void CalendarPreviousButtonAccessibleObject_ctor_default()
    {
        using MonthCalendar control = new();
        var controlAccessibleObject = (MonthCalendarAccessibleObject)control.AccessibilityObject;
        CalendarPreviousButtonAccessibleObject previousButtonAccessibleObject = new(controlAccessibleObject);

        Assert.Equal(controlAccessibleObject, previousButtonAccessibleObject.TestAccessor().Dynamic._monthCalendarAccessibleObject);
        Assert.False(control.IsHandleCreated);
    }

    [WinFormsFact]
    public void CalendarPreviousButtonAccessibleObject_Description_ReturnsExpected()
    {
        using MonthCalendar control = new();
        var controlAccessibleObject = (MonthCalendarAccessibleObject)control.AccessibilityObject;
        CalendarPreviousButtonAccessibleObject previousButtonAccessibleObject = new(controlAccessibleObject);

        string actual = previousButtonAccessibleObject.Description;

        Assert.Equal(SR.CalendarPreviousButtonAccessibleObjectDescription, actual);
        Assert.False(control.IsHandleCreated);
    }

    [WinFormsFact]
    public void CalendarPreviousButtonAccessibleObject_GetChildId_ReturnsExpected()
    {
        using MonthCalendar control = new();
        var controlAccessibleObject = (MonthCalendarAccessibleObject)control.AccessibilityObject;
        CalendarPreviousButtonAccessibleObject previousButtonAccessibleObject = new(controlAccessibleObject);

        int actual = previousButtonAccessibleObject.GetChildId();

        Assert.Equal(1, actual);
        Assert.False(control.IsHandleCreated);
    }

    [WinFormsFact]
    public void CalendarPreviousButtonAccessibleObject_Name_ReturnsExpected()
    {
        using MonthCalendar control = new();
        var controlAccessibleObject = (MonthCalendarAccessibleObject)control.AccessibilityObject;
        CalendarPreviousButtonAccessibleObject previousButtonAccessibleObject = new(controlAccessibleObject);

        string actual = previousButtonAccessibleObject.Name;

        Assert.Equal(SR.MonthCalendarPreviousButtonAccessibleName, actual);
        Assert.False(control.IsHandleCreated);
    }

    [WinFormsFact]
    public void CalendarPreviousButtonAccessibleObject_FragmentNavigate_Parent_ReturnsExpected()
    {
        using MonthCalendar control = new();
        MonthCalendarAccessibleObject controlAccessibleObject = new(control);
        CalendarPreviousButtonAccessibleObject prevButton = new(controlAccessibleObject);

        Assert.Equal(controlAccessibleObject, prevButton.FragmentNavigate(Interop.UiaCore.NavigateDirection.Parent));
        Assert.False(control.IsHandleCreated);
    }

    [WinFormsFact]
    public void CalendarPreviousButtonAccessibleObject_FragmentNavigate_Sibling_ReturnsExpected()
    {
        using MonthCalendar control = new();
        MonthCalendarAccessibleObject controlAccessibleObject = new(control);
        CalendarPreviousButtonAccessibleObject prevButton = new(controlAccessibleObject);

        AccessibleObject nextButton = controlAccessibleObject.NextButtonAccessibleObject;

        Assert.Null(prevButton.FragmentNavigate(Interop.UiaCore.NavigateDirection.PreviousSibling));
        Assert.Equal(nextButton, prevButton.FragmentNavigate(Interop.UiaCore.NavigateDirection.NextSibling));
        Assert.False(control.IsHandleCreated);
    }

    [WinFormsFact]
    public void CalendarPreviousButtonAccessibleObject_FragmentNavigate_Child_ReturnsExpected()
    {
        using MonthCalendar control = new();
        MonthCalendarAccessibleObject controlAccessibleObject = new(control);
        CalendarPreviousButtonAccessibleObject prevButton = new(controlAccessibleObject);

        Assert.Null(prevButton.FragmentNavigate(Interop.UiaCore.NavigateDirection.FirstChild));
        Assert.Null(prevButton.FragmentNavigate(Interop.UiaCore.NavigateDirection.LastChild));
        Assert.False(control.IsHandleCreated);
    }
}