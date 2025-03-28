﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel;
using System.Drawing;
using Moq;

namespace System.Windows.Forms.Tests;

public class NotifyIconTests
{
    [WinFormsFact]
    public void NotifyIcon_Ctor_Default()
    {
        using NotifyIcon notifyIcon = new();
        Assert.Equal(ToolTipIcon.None, notifyIcon.BalloonTipIcon);
        Assert.Empty(notifyIcon.BalloonTipText);
        Assert.Empty(notifyIcon.BalloonTipTitle);
        Assert.Null(notifyIcon.Container);
        Assert.Null(notifyIcon.ContextMenuStrip);
        Assert.Null(notifyIcon.Icon);
        Assert.Null(notifyIcon.Site);
        Assert.Null(notifyIcon.Tag);
        Assert.Empty(notifyIcon.Text);
        Assert.False(notifyIcon.Visible);
    }

    [WinFormsFact]
    public void NotifyIcon_Ctor_IContainer()
    {
        Container container = new();
        using NotifyIcon notifyIcon = new(container);
        Assert.Equal(ToolTipIcon.None, notifyIcon.BalloonTipIcon);
        Assert.Empty(notifyIcon.BalloonTipText);
        Assert.Empty(notifyIcon.BalloonTipTitle);
        Assert.Same(container, notifyIcon.Container);
        Assert.Null(notifyIcon.ContextMenuStrip);
        Assert.Null(notifyIcon.Icon);
        Assert.NotNull(notifyIcon.Site);
        Assert.Null(notifyIcon.Tag);
        Assert.Empty(notifyIcon.Text);
        Assert.False(notifyIcon.Visible);
    }

    [WinFormsFact]
    public void NotifyIcon_Ctor_NullContainer_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>("container", () => new NotifyIcon(null));
    }

    [WinFormsTheory]
    [EnumData<ToolTipIcon>]
    public void NotifyIcon_BalloonTipIcon_Set_GetReturnsExpected(ToolTipIcon value)
    {
        NotifyIcon notifyIcon = new()
        {
            BalloonTipIcon = value
        };
        Assert.Equal(value, notifyIcon.BalloonTipIcon);

        // Set same.
        notifyIcon.BalloonTipIcon = value;
        Assert.Equal(value, notifyIcon.BalloonTipIcon);
    }

    [WinFormsTheory]
    [InvalidEnumData<ToolTipIcon>]
    public void NotifyIcon_BalloonTipIcon_SetInvalidValue_ThrowsInvalidEnumArgumentException(ToolTipIcon value)
    {
        NotifyIcon notifyIcon = new();
        Assert.Throws<InvalidEnumArgumentException>("value", () => notifyIcon.BalloonTipIcon = value);
    }

    [WinFormsTheory]
    [StringWithNullData]
    public void CollectionForm_BalloonTipText_Set_GetReturnsExpected(string value)
    {
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipText = value
        };
        Assert.Equal(value, notifyIcon.BalloonTipText);

        // Set same.
        notifyIcon.BalloonTipText = value;
        Assert.Equal(value, notifyIcon.BalloonTipText);
    }

    [WinFormsTheory]
    [StringWithNullData]
    public void CollectionForm_BalloonTipText_SetWithCustomOldValue_GetReturnsExpected(string value)
    {
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipText = "OldValue"
        };

        notifyIcon.BalloonTipText = value;
        Assert.Equal(value, notifyIcon.BalloonTipText);

        // Set same.
        notifyIcon.BalloonTipText = value;
        Assert.Equal(value, notifyIcon.BalloonTipText);
    }

    [WinFormsTheory]
    [StringWithNullData]
    public void CollectionForm_BalloonTipTitle_Set_GetReturnsExpected(string value)
    {
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipTitle = value
        };
        Assert.Equal(value, notifyIcon.BalloonTipTitle);

        // Set same.
        notifyIcon.BalloonTipTitle = value;
        Assert.Equal(value, notifyIcon.BalloonTipTitle);
    }

    [WinFormsTheory]
    [StringWithNullData]
    public void CollectionForm_BalloonTipTitle_SetWithCustomOldValue_GetReturnsExpected(string value)
    {
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipTitle = "OldValue"
        };

        notifyIcon.BalloonTipTitle = value;
        Assert.Equal(value, notifyIcon.BalloonTipTitle);

        // Set same.
        notifyIcon.BalloonTipTitle = value;
        Assert.Equal(value, notifyIcon.BalloonTipTitle);
    }

    public static IEnumerable<object[]> ContextMenuStrip_Set_TestData()
    {
        yield return new object[] { null };
        yield return new object[] { new ContextMenuStrip() };
    }

    [WinFormsTheory]
    [MemberData(nameof(ContextMenuStrip_Set_TestData))]
    public void CollectionForm_ContextMenuStrip_Set_GetReturnsExpected(ContextMenuStrip value)
    {
        using NotifyIcon notifyIcon = new()
        {
            ContextMenuStrip = value
        };
        Assert.Equal(value, notifyIcon.ContextMenuStrip);

        // Set same.
        notifyIcon.ContextMenuStrip = value;
        Assert.Equal(value, notifyIcon.ContextMenuStrip);
    }

    [WinFormsTheory]
    [MemberData(nameof(ContextMenuStrip_Set_TestData))]
    public void CollectionForm_ContextMenuStrip_SetWithCustomOldValue_GetReturnsExpected(ContextMenuStrip value)
    {
        using ContextMenuStrip menu = new();
        using NotifyIcon notifyIcon = new()
        {
            ContextMenuStrip = menu
        };

        notifyIcon.ContextMenuStrip = value;
        Assert.Equal(value, notifyIcon.ContextMenuStrip);

        // Set same.
        notifyIcon.ContextMenuStrip = value;
        Assert.Equal(value, notifyIcon.ContextMenuStrip);
    }

    [WinFormsTheory]
    [MemberData(nameof(ContextMenuStrip_Set_TestData))]
    public void CollectionForm_ContextMenuStrip_SetDisposed_GetReturnsExpected(ContextMenuStrip value)
    {
        using NotifyIcon notifyIcon = new();
        notifyIcon.Dispose();

        notifyIcon.ContextMenuStrip = value;
        Assert.Equal(value, notifyIcon.ContextMenuStrip);

        // Set same.
        notifyIcon.ContextMenuStrip = value;
        Assert.Equal(value, notifyIcon.ContextMenuStrip);
    }

    public static IEnumerable<object[]> Icon_Set_TestData()
    {
        yield return new object[] { null };
        yield return new object[] { new Icon("bitmaps/10x16_one_entry_32bit.ico") };
    }

    [WinFormsTheory]
    [MemberData(nameof(Icon_Set_TestData))]
    public void NotifyIcon_Icon_Set_GetReturnsExpected(Icon value)
    {
        using NotifyIcon notifyIcon = new()
        {
            Icon = value
        };
        Assert.Same(value, notifyIcon.Icon);

        // Set same.
        notifyIcon.Icon = value;
        Assert.Same(value, notifyIcon.Icon);
    }

    [WinFormsTheory]
    [MemberData(nameof(Icon_Set_TestData))]
    public void NotifyIcon_Icon_SetWithCustomOldValue_GetReturnsExpected(Icon value)
    {
        using Icon oldValue = new(new Icon("bitmaps/10x16_one_entry_32bit.ico"), 10, 10);
        using NotifyIcon notifyIcon = new()
        {
            Icon = oldValue
        };

        notifyIcon.Icon = value;
        Assert.Same(value, notifyIcon.Icon);

        // Set same.
        notifyIcon.Icon = value;
        Assert.Same(value, notifyIcon.Icon);
    }

    [WinFormsTheory]
    [MemberData(nameof(Icon_Set_TestData))]
    public void NotifyIcon_Icon_SetWithVisible_GetReturnsExpected(Icon value)
    {
        using NotifyIcon notifyIcon = new()
        {
            Visible = true,
            Icon = value
        };
        Assert.Same(value, notifyIcon.Icon);

        // Set same.
        notifyIcon.Icon = value;
        Assert.Same(value, notifyIcon.Icon);
    }

    public static IEnumerable<object[]> Icon_SetDesignMode_TestData()
    {
        yield return new object[] { null, 0 };
        yield return new object[] { new Icon("bitmaps/10x16_one_entry_32bit.ico"), 1 };
    }

    [WinFormsTheory]
    [MemberData(nameof(Icon_SetDesignMode_TestData))]
    public void NotifyIcon_Icon_SetDesignMode_GetReturnsExpected(Icon value, int expectedDesignModeCallCount)
    {
        Mock<ISite> mockSite = new(MockBehavior.Strict);
        mockSite
            .Setup(s => s.Container)
            .Returns((IContainer)null);
        mockSite
            .Setup(s => s.DesignMode)
            .Returns(true);
        using NotifyIcon notifyIcon = new()
        {
            Site = mockSite.Object,
            Icon = value
        };
        Assert.Same(value, notifyIcon.Icon);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));

        // Set same.
        notifyIcon.Icon = value;
        Assert.Same(value, notifyIcon.Icon);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));
    }

    [WinFormsTheory]
    [MemberData(nameof(Icon_SetDesignMode_TestData))]
    public void NotifyIcon_Icon_SetDesignModeWithVisible_GetReturnsExpected(Icon value, int expectedDesignModeCallCount)
    {
        Mock<ISite> mockSite = new(MockBehavior.Strict);
        mockSite
            .Setup(s => s.Container)
            .Returns((IContainer)null);
        mockSite
            .Setup(s => s.DesignMode)
            .Returns(true);
        using NotifyIcon notifyIcon = new()
        {
            Visible = true,
            Site = mockSite.Object,
            Icon = value
        };
        Assert.Same(value, notifyIcon.Icon);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));

        // Set same.
        notifyIcon.Icon = value;
        Assert.Same(value, notifyIcon.Icon);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));
    }

    [WinFormsFact]
    public void NotifyIcon_Icon_SetDisposed_ThrowsNullReferenceException()
    {
        using NotifyIcon notifyIcon = new();
        notifyIcon.Dispose();
        using Icon value = new("bitmaps/10x16_one_entry_32bit.ico");
        Assert.Throws<NullReferenceException>(() => notifyIcon.Icon = value);
        Assert.Same(value, notifyIcon.Icon);
    }

    [WinFormsTheory]
    [StringData]
    public void CollectionForm_Tag_Set_GetReturnsExpected(object value)
    {
        using NotifyIcon notifyIcon = new()
        {
            Tag = value
        };
        Assert.Same(value, notifyIcon.Tag);

        // Set same.
        notifyIcon.Tag = value;
        Assert.Same(value, notifyIcon.Tag);
    }

    public static IEnumerable<object[]> Text_Set_TestData()
    {
        foreach (bool visible in new bool[] { true, false })
        {
            foreach (Icon icon in new Icon[] { null, new("bitmaps/10x16_one_entry_32bit.ico") })
            {
                yield return new object[] { visible, icon, null, string.Empty };
                yield return new object[] { visible, icon, string.Empty, string.Empty };
                yield return new object[] { visible, icon, "text", "text" };
                yield return new object[] { visible, icon, new string('a', NotifyIcon.MaxTextSize), new string('a', NotifyIcon.MaxTextSize) };
            }
        }
    }

    [WinFormsTheory]
    [MemberData(nameof(Text_Set_TestData))]
    public void CollectionForm_Text_Set_GetReturnsExpected(bool visible, Icon icon, string value, string expected)
    {
        using NotifyIcon notifyIcon = new()
        {
            Visible = visible,
            Icon = icon,
            Text = value
        };
        Assert.Equal(expected, notifyIcon.Text);

        // Set same.
        notifyIcon.Text = value;
        Assert.Equal(expected, notifyIcon.Text);
    }

    [WinFormsTheory]
    [MemberData(nameof(Text_Set_TestData))]
    public void CollectionForm_Text_SetWithCustomOldValue_GetReturnsExpected(bool visible, Icon icon, string value, string expected)
    {
        using NotifyIcon notifyIcon = new()
        {
            Visible = visible,
            Icon = icon,
            Text = "OldValue"
        };

        notifyIcon.Text = value;
        Assert.Equal(expected, notifyIcon.Text);

        // Set same.
        notifyIcon.Text = value;
        Assert.Equal(expected, notifyIcon.Text);
    }

    public static IEnumerable<object[]> Text_SetDesignMode_TestData()
    {
        foreach (bool visible in new bool[] { true, false })
        {
            foreach (Icon icon in new Icon[] { null, new("bitmaps/10x16_one_entry_32bit.ico") })
            {
                yield return new object[] { visible, icon, null, string.Empty, 0 };
                yield return new object[] { visible, icon, string.Empty, string.Empty, 0 };
            }

            yield return new object[] { visible, null, "text", "text", 0 };
            yield return new object[] { visible, null, new string('a', NotifyIcon.MaxTextSize), new string('a', NotifyIcon.MaxTextSize), 0 };
        }

        yield return new object[] { false, new Icon("bitmaps/10x16_one_entry_32bit.ico"), "text", "text", 0 };
        yield return new object[] { false, new Icon("bitmaps/10x16_one_entry_32bit.ico"), new string('a', NotifyIcon.MaxTextSize), new string('a', NotifyIcon.MaxTextSize), 0 };
        yield return new object[] { true, new Icon("bitmaps/10x16_one_entry_32bit.ico"), "text", "text", 1 };
        yield return new object[] { true, new Icon("bitmaps/10x16_one_entry_32bit.ico"), new string('a', NotifyIcon.MaxTextSize), new string('a', NotifyIcon.MaxTextSize), 1 };
    }

    [WinFormsTheory]
    [MemberData(nameof(Text_SetDesignMode_TestData))]
    public void CollectionForm_Text_SetDesignMode_GetReturnsExpected(bool visible, Icon icon, string value, string expected, int expectedDesignModeCallCount)
    {
        Mock<ISite> mockSite = new(MockBehavior.Strict);
        mockSite
            .Setup(s => s.Container)
            .Returns((IContainer)null);
        mockSite
            .Setup(s => s.DesignMode)
            .Returns(true);
        using NotifyIcon notifyIcon = new()
        {
            Visible = visible,
            Icon = icon,
            Site = mockSite.Object,
            Text = value
        };
        Assert.Equal(expected, notifyIcon.Text);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));

        // Set same.
        notifyIcon.Text = value;
        Assert.Equal(expected, notifyIcon.Text);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));
    }

    [WinFormsFact]
    public void NotifyIcon_Text_SetLongValue_ThrowsArgumentOutOfRangeException()
    {
        using NotifyIcon notifyIcon = new();
        Assert.Throws<ArgumentOutOfRangeException>("Text", () => notifyIcon.Text = new string('a', NotifyIcon.MaxTextSize + 1));
    }

    [WinFormsTheory]
    [MemberData(nameof(Text_Set_TestData))]
    public void CollectionForm_Text_SetDisposed_GetReturnsExpected(bool visible, Icon icon, string value, string expected)
    {
        using NotifyIcon notifyIcon = new()
        {
            Visible = visible,
            Icon = icon
        };
        notifyIcon.Dispose();

        notifyIcon.Text = value;
        Assert.Equal(expected, notifyIcon.Text);

        // Set same.
        notifyIcon.Text = value;
        Assert.Equal(expected, notifyIcon.Text);
    }

    [WinFormsTheory]
    [BoolData]
    public void NotifyIcon_Visible_Set_GetReturnsExpected(bool value)
    {
        using NotifyIcon notifyIcon = new()
        {
            Visible = value
        };
        Assert.Equal(value, notifyIcon.Visible);

        // Set same.
        notifyIcon.Visible = value;
        Assert.Equal(value, notifyIcon.Visible);

        // Set different.
        notifyIcon.Visible = !value;
        Assert.Equal(!value, notifyIcon.Visible);
    }

    [WinFormsTheory]
    [BoolData]
    public void NotifyIcon_Visible_SetWithIcon_GetReturnsExpected(bool value)
    {
        using Icon icon = new("bitmaps/10x16_one_entry_32bit.ico");
        using NotifyIcon notifyIcon = new()
        {
            Icon = icon,
            Visible = value
        };
        Assert.Equal(value, notifyIcon.Visible);

        // Set same.
        notifyIcon.Visible = value;
        Assert.Equal(value, notifyIcon.Visible);

        // Set different.
        notifyIcon.Visible = !value;
        Assert.Equal(!value, notifyIcon.Visible);
    }

    [WinFormsTheory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void NotifyIcon_Visible_SetDesignMode_GetReturnsExpected(bool value, int expectedDesignModeCallCount)
    {
        Mock<ISite> mockSite = new(MockBehavior.Strict);
        mockSite
            .Setup(s => s.Container)
            .Returns((IContainer)null);
        mockSite
            .Setup(s => s.DesignMode)
            .Returns(true);
        using NotifyIcon notifyIcon = new()
        {
            Site = mockSite.Object,
            Visible = value
        };
        Assert.Equal(value, notifyIcon.Visible);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));

        // Set same.
        notifyIcon.Visible = value;
        Assert.Equal(value, notifyIcon.Visible);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));

        // Set different.
        notifyIcon.Visible = !value;
        Assert.Equal(!value, notifyIcon.Visible);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount + 1));
    }

    [WinFormsTheory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void NotifyIcon_Visible_SetDesignModeWithIcon_GetReturnsExpected(bool value, int expectedDesignModeCallCount)
    {
        Mock<ISite> mockSite = new(MockBehavior.Strict);
        mockSite
            .Setup(s => s.Container)
            .Returns((IContainer)null);
        mockSite
            .Setup(s => s.DesignMode)
            .Returns(true);
        using Icon icon = new("bitmaps/10x16_one_entry_32bit.ico");
        using NotifyIcon notifyIcon = new()
        {
            Icon = icon,
            Site = mockSite.Object,
            Visible = value
        };
        Assert.Equal(value, notifyIcon.Visible);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));

        // Set same.
        notifyIcon.Visible = value;
        Assert.Equal(value, notifyIcon.Visible);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount));

        // Set different.
        notifyIcon.Visible = !value;
        Assert.Equal(!value, notifyIcon.Visible);
        mockSite.Verify(s => s.DesignMode, Times.Exactly(expectedDesignModeCallCount + 1));
    }

    [WinFormsTheory]
    [BoolData]
    public void NotifyIcon_Visible_SetDisposed_ThrowsNullReferenceException(bool value)
    {
        using NotifyIcon notifyIcon = new()
        {
            Visible = value
        };
        notifyIcon.Dispose();
        Assert.Throws<NullReferenceException>(() => notifyIcon.Visible = !value);
        Assert.Equal(value, notifyIcon.Visible);
    }

    [WinFormsFact]
    public void NotifyIcon_BalloonTipClicked_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, EventArgs e)
        {
            callCount++;
        }

        notifyIcon.BalloonTipClicked += handler;
        notifyIcon.BalloonTipClicked -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_BalloonTipClosed_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, EventArgs e)
        {
            callCount++;
        }

        notifyIcon.BalloonTipClosed += handler;
        notifyIcon.BalloonTipClosed -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_BalloonTipShown_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, EventArgs e)
        {
            callCount++;
        }

        notifyIcon.BalloonTipShown += handler;
        notifyIcon.BalloonTipShown -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_Click_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, EventArgs e)
        {
            callCount++;
        }

        notifyIcon.Click += handler;
        notifyIcon.Click -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_DoubleClick_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, EventArgs e)
        {
            callCount++;
        }

        notifyIcon.DoubleClick += handler;
        notifyIcon.DoubleClick -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_MouseClick_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, MouseEventArgs e)
        {
            callCount++;
        }

        notifyIcon.MouseClick += handler;
        notifyIcon.MouseClick -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_MouseDoubleClick_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, MouseEventArgs e)
        {
            callCount++;
        }

        notifyIcon.MouseDoubleClick += handler;
        notifyIcon.MouseDoubleClick -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_MouseDown_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, MouseEventArgs e)
        {
            callCount++;
        }

        notifyIcon.MouseDown += handler;
        notifyIcon.MouseDown -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_MouseMove_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, MouseEventArgs e)
        {
            callCount++;
        }

        notifyIcon.MouseMove += handler;
        notifyIcon.MouseMove -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_MouseUp_AddRemoveEvent_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        void handler(object sender, MouseEventArgs e)
        {
            callCount++;
        }

        notifyIcon.MouseUp += handler;
        notifyIcon.MouseUp -= handler;
        Assert.Equal(0, callCount);
    }

    [WinFormsFact]
    public void NotifyIcon_Dispose_Invoke_Success()
    {
        using NotifyIcon notifyIcon = new();
        int callCount = 0;
        notifyIcon.Disposed += (sender, e) => callCount++;

        notifyIcon.Dispose();

        Assert.Equal(ToolTipIcon.None, notifyIcon.BalloonTipIcon);
        Assert.Empty(notifyIcon.BalloonTipText);
        Assert.Empty(notifyIcon.BalloonTipTitle);
        Assert.Null(notifyIcon.Container);
        Assert.Null(notifyIcon.ContextMenuStrip);
        Assert.Null(notifyIcon.Icon);
        Assert.Null(notifyIcon.Site);
        Assert.Null(notifyIcon.Tag);
        Assert.Empty(notifyIcon.Text);
        Assert.False(notifyIcon.Visible);
        Assert.Equal(1, callCount);

        notifyIcon.Dispose();

        Assert.Equal(ToolTipIcon.None, notifyIcon.BalloonTipIcon);
        Assert.Empty(notifyIcon.BalloonTipText);
        Assert.Empty(notifyIcon.BalloonTipTitle);
        Assert.Null(notifyIcon.Container);
        Assert.Null(notifyIcon.ContextMenuStrip);
        Assert.Null(notifyIcon.Icon);
        Assert.Null(notifyIcon.Site);
        Assert.Null(notifyIcon.Tag);
        Assert.Empty(notifyIcon.Text);
        Assert.False(notifyIcon.Visible);
        Assert.Equal(2, callCount);
    }

    public static IEnumerable<object[]> Dispose_WithProperties_TestData()
    {
        foreach (bool visible in new bool[] { true, false })
        {
            foreach (Icon icon in new Icon[] { null, new("bitmaps/10x16_one_entry_32bit.ico") })
            {
                yield return new object[] { visible, icon };
            }
        }
    }

    [WinFormsTheory]
    [MemberData(nameof(Dispose_WithProperties_TestData))]
    public void NotifyIcon_Dispose_InvokePropertiesSet_Success(bool visible, Icon icon)
    {
        using ContextMenuStrip contextMenuStrip = new();
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipIcon = ToolTipIcon.Error,
            BalloonTipText = "BalloonTipText",
            BalloonTipTitle = "BalloonTipTitle",
            ContextMenuStrip = contextMenuStrip,
            Icon = icon,
            Text = "Text",
            Tag = "Tag",
            Visible = visible
        };
        int callCount = 0;
        notifyIcon.Disposed += (sender, e) => callCount++;

        notifyIcon.Dispose();
        Assert.Equal(ToolTipIcon.Error, notifyIcon.BalloonTipIcon);
        Assert.Equal("BalloonTipText", notifyIcon.BalloonTipText);
        Assert.Equal("BalloonTipTitle", notifyIcon.BalloonTipTitle);
        Assert.Null(notifyIcon.Container);
        Assert.Null(notifyIcon.ContextMenuStrip);
        Assert.Null(notifyIcon.Icon);
        Assert.Null(notifyIcon.Site);
        Assert.Equal("Tag", notifyIcon.Tag);
        Assert.Empty(notifyIcon.Text);
        Assert.Equal(visible, notifyIcon.Visible);
        Assert.Equal(1, callCount);

        notifyIcon.Dispose();
        Assert.Equal(ToolTipIcon.Error, notifyIcon.BalloonTipIcon);
        Assert.Equal("BalloonTipText", notifyIcon.BalloonTipText);
        Assert.Equal("BalloonTipTitle", notifyIcon.BalloonTipTitle);
        Assert.Null(notifyIcon.Container);
        Assert.Null(notifyIcon.ContextMenuStrip);
        Assert.Null(notifyIcon.Icon);
        Assert.Null(notifyIcon.Site);
        Assert.Equal("Tag", notifyIcon.Tag);
        Assert.Empty(notifyIcon.Text);
        Assert.Equal(visible, notifyIcon.Visible);
        Assert.Equal(2, callCount);
    }

    public static IEnumerable<object[]> ShowBalloonTip_TestData()
    {
        foreach (ToolTipIcon tipIcon in Enum.GetValues(typeof(ToolTipIcon)))
        {
            yield return new object[] { 0, tipIcon };
        }
    }

    [WinFormsTheory]
    [MemberData(nameof(ShowBalloonTip_TestData))]
    public void NotifyIcon_ShowBalloonTip_InvokeInt_Success(int timeout, ToolTipIcon tipIcon)
    {
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipTitle = "BalloonTipTitle",
            BalloonTipText = "BalloonTipText",
            BalloonTipIcon = tipIcon
        };
        notifyIcon.ShowBalloonTip(timeout);
    }

    [WinFormsTheory]
    [MemberData(nameof(ShowBalloonTip_TestData))]
    public void NotifyIcon_ShowBalloonTip_InvokeIntAdded_Success(int timeout, ToolTipIcon tipIcon)
    {
        using Icon icon = new("bitmaps/10x16_one_entry_32bit.ico");
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipTitle = "BalloonTipTitle",
            BalloonTipText = "BalloonTipText",
            BalloonTipIcon = tipIcon,
            Icon = icon,
            Visible = true
        };
        notifyIcon.ShowBalloonTip(timeout);
    }

    [WinFormsTheory]
    [MemberData(nameof(ShowBalloonTip_TestData))]
    public void NotifyIcon_ShowBalloonTip_InvokeIntAddedDesignMode_Success(int timeout, ToolTipIcon tipIcon)
    {
        using Icon icon = new("bitmaps/10x16_one_entry_32bit.ico");
        Mock<ISite> mockSite = new(MockBehavior.Strict);
        mockSite
            .Setup(s => s.Container)
            .Returns((IContainer)null);
        mockSite
            .Setup(s => s.DesignMode)
            .Returns(true);
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipTitle = "BalloonTipTitle",
            BalloonTipText = "BalloonTipText",
            BalloonTipIcon = tipIcon,
            Icon = icon,
            Visible = true,
            Site = mockSite.Object
        };
        notifyIcon.ShowBalloonTip(timeout);
        mockSite.Verify(s => s.DesignMode, Times.Once());
    }

    [WinFormsTheory]
    [MemberData(nameof(ShowBalloonTip_TestData))]
    public void NotifyIcon_ShowBalloonTip_InvokeIntDisposed_Success(int timeout, ToolTipIcon tipIcon)
    {
        using Icon icon = new("bitmaps/10x16_one_entry_32bit.ico");
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipTitle = "BalloonTipTitle",
            BalloonTipText = "BalloonTipText",
            BalloonTipIcon = tipIcon,
            Icon = icon,
            Visible = true
        };
        notifyIcon.Dispose();

        notifyIcon.ShowBalloonTip(timeout);
    }

    [WinFormsTheory]
    [MemberData(nameof(ShowBalloonTip_TestData))]
    public void NotifyIcon_ShowBalloonTip_InvokeIntStringStringToolTipIcon_Success(int timeout, ToolTipIcon tipIcon)
    {
        using NotifyIcon notifyIcon = new();
        notifyIcon.ShowBalloonTip(timeout, "tipTitle", "tipText", tipIcon);
    }

    [WinFormsTheory]
    [MemberData(nameof(ShowBalloonTip_TestData))]
    public void NotifyIcon_ShowBalloonTip_InvokeIntStringStringToolTipIconAdded_Success(int timeout, ToolTipIcon tipIcon)
    {
        using Icon icon = new("bitmaps/10x16_one_entry_32bit.ico");
        using NotifyIcon notifyIcon = new()
        {
            Icon = icon,
            Visible = true
        };
        notifyIcon.ShowBalloonTip(timeout, "tipTitle", "tipText", tipIcon);
    }

    [WinFormsTheory]
    [MemberData(nameof(ShowBalloonTip_TestData))]
    public void NotifyIcon_ShowBalloonTip_InvokeIntStringStringToolTipIconDisposed_Success(int timeout, ToolTipIcon tipIcon)
    {
        using Icon icon = new("bitmaps/10x16_one_entry_32bit.ico");
        using NotifyIcon notifyIcon = new()
        {
            Icon = icon,
            Visible = true
        };
        notifyIcon.Dispose();
        notifyIcon.ShowBalloonTip(timeout, "tipTitle", "tipText", tipIcon);
    }

    [WinFormsTheory]
    [MemberData(nameof(ShowBalloonTip_TestData))]
    public void NotifyIcon_ShowBalloonTip_InvokeIntStringStringToolTipIconAddedDesignMode_Success(int timeout, ToolTipIcon tipIcon)
    {
        using Icon icon = new("bitmaps/10x16_one_entry_32bit.ico");
        Mock<ISite> mockSite = new(MockBehavior.Strict);
        mockSite
            .Setup(s => s.Container)
            .Returns((IContainer)null);
        mockSite
            .Setup(s => s.DesignMode)
            .Returns(true);
        using NotifyIcon notifyIcon = new()
        {
            Icon = icon,
            Visible = true,
            Site = mockSite.Object
        };
        notifyIcon.ShowBalloonTip(timeout, "tipTitle", "tipText", tipIcon);
        mockSite.Verify(s => s.DesignMode, Times.Once());
    }

    [WinFormsFact]
    public void NotifyIcon_ShowBalloonTip_InvokeNegativeTimeout_ThrowsArgumentOutOfRangeException()
    {
        using NotifyIcon notifyIcon = new();
        Assert.Throws<ArgumentOutOfRangeException>("timeout", () => notifyIcon.ShowBalloonTip(-1));
        Assert.Throws<ArgumentOutOfRangeException>("timeout", () => notifyIcon.ShowBalloonTip(-1, "Title", "Text", ToolTipIcon.Error));
    }

    [WinFormsTheory]
    [NullAndEmptyStringData]
    public void NotifyIcon_ShowBalloonTip_InvokeInvalidText_ThrowsArgumentException(string tipText)
    {
        using NotifyIcon notifyIcon = new()
        {
            BalloonTipText = tipText
        };
        Assert.Throws<ArgumentException>(() => notifyIcon.ShowBalloonTip(0));
        Assert.Throws<ArgumentException>(() => notifyIcon.ShowBalloonTip(0, "Title", tipText, ToolTipIcon.Error));
    }

    [WinFormsTheory]
    [InvalidEnumData<ToolTipIcon>]
    public void NotifyIcon_ShowBalloonTip_InvokeInvalidTipIcon_ThrowsInvalidEnumArgumentException(ToolTipIcon tipIcon)
    {
        using NotifyIcon notifyIcon = new();
        Assert.Throws<InvalidEnumArgumentException>("tipIcon", () => notifyIcon.ShowBalloonTip(0, "Title", "Text", tipIcon));
    }
}
