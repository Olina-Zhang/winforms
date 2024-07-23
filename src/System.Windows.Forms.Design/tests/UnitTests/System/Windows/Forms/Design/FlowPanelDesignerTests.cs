﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using Moq;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design.Tests;

public sealed class FlowPanelDesignerTests
{
    private (Panel panel, FlowPanelDesigner designer) SetupDesignerWithPanel()
    {
        var mockHost = new Mock<IDesignerHost>();
        var mockContainer = new Mock<IContainer>();
        mockHost.Setup(host => host.Container).Returns(mockContainer.Object);

        Panel panel = new();
        mockContainer.Object.Add(panel);

        FlowPanelDesigner designer = new();
        designer.Initialize(panel);

        return (panel, designer);
    }

    [Fact]
    public void ParticipatesWithSnapLines_ShouldAlwaysReturnFalse()
    {
        var (_, designer) = SetupDesignerWithPanel();

        designer.ParticipatesWithSnapLines.Should().BeFalse();
    }

    [Fact]
    public void SnapLines_ShouldNotContainPaddingSnapLines()
    {
        var (_, designer) = SetupDesignerWithPanel();

        var snapLines = designer.SnapLines as IList<SnapLine>;

        snapLines.Should().NotBeNull();
        snapLines.Should().NotContain(line => line.Filter is object && line.Filter.Contains(SnapLine.Padding));
    }

    [Fact]
    public void SnapLines_ShouldReturnNonPaddingSnapLines()
    {
        var (_, designer) = SetupDesignerWithPanel();

        var snapLines = designer.SnapLines as IList<SnapLine>;

        snapLines.Should().NotBeNull();
        snapLines.Should().Contain(line => line.Filter == null || !line.Filter.Contains(SnapLine.Padding));
    }
}
