﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Specialized;

namespace System.Windows.Forms;

// this class encapsulates the logic for Renderer and RenderMode so it can
// be shared across classes.
internal class ToolStripRendererSwitcher
{
    private static readonly int stateUseDefaultRenderer = BitVector32.CreateMask();
    private static readonly int stateAttachedRendererChanged = BitVector32.CreateMask(stateUseDefaultRenderer);

    private ToolStripRenderer? _renderer;
    private Type _currentRendererType = typeof(Type);
    private BitVector32 _state;

    private readonly ToolStripRenderMode _defaultRenderMode = ToolStripRenderMode.ManagerRenderMode;

    public ToolStripRendererSwitcher(Control owner, ToolStripRenderMode defaultRenderMode) : this(owner)
    {
        _defaultRenderMode = defaultRenderMode;
        RenderMode = defaultRenderMode;
    }

    public ToolStripRendererSwitcher(Control owner)
    {
        _state[stateUseDefaultRenderer] = true;
        _state[stateAttachedRendererChanged] = false;
        owner.Disposed += new EventHandler(OnControlDisposed);
        owner.VisibleChanged += new EventHandler(OnControlVisibleChanged);
        if (owner.Visible)
        {
            OnControlVisibleChanged(owner, EventArgs.Empty);
        }
    }

    public ToolStripRenderer Renderer
    {
        get
        {
            if (RenderMode == ToolStripRenderMode.ManagerRenderMode)
            {
                return ToolStripManager.Renderer;
            }

            // always return a valid renderer so our paint code
            // doesn't have to be bogged down by checks for null.

            _state[stateUseDefaultRenderer] = false;
            if (_renderer is null)
            {
                Renderer = ToolStripManager.CreateRenderer(RenderMode);
            }

            return _renderer!;
        }
        set
        {
            // if the value happens to be null, the next get
            // will autogenerate a new ToolStripRenderer.
            if (_renderer != value)
            {
                _state[stateUseDefaultRenderer] = value is null;
                _renderer = value;
                _currentRendererType = (_renderer is not null) ? _renderer.GetType() : typeof(Type);

                OnRendererChanged(EventArgs.Empty);
            }
        }
    }

    public ToolStripRenderMode RenderMode
    {
        get
        {
            if (_state[stateUseDefaultRenderer])
            {
                return ToolStripRenderMode.ManagerRenderMode;
            }

            if (_renderer is not null && !_renderer.IsAutoGenerated)
            {
                return ToolStripRenderMode.Custom;
            }

            // check the type of the currently set renderer.
            // types are cached as this may be called frequently.
            if (_currentRendererType == ToolStripManager.s_professionalRendererType)
            {
                return ToolStripRenderMode.Professional;
            }

            if (_currentRendererType == ToolStripManager.s_systemRendererType)
            {
                return ToolStripRenderMode.System;
            }

            return ToolStripRenderMode.Custom;
        }
        set
        {
            // valid values are 0x0 to 0x3
            SourceGenerated.EnumValidator.Validate(value);
            if (value == ToolStripRenderMode.Custom)
            {
                throw new NotSupportedException(SR.ToolStripRenderModeUseRendererPropertyInstead);
            }

            if (value == ToolStripRenderMode.ManagerRenderMode)
            {
                if (!_state[stateUseDefaultRenderer])
                {
                    _state[stateUseDefaultRenderer] = true;
                    OnRendererChanged(EventArgs.Empty);
                }
            }
            else
            {
                _state[stateUseDefaultRenderer] = false;
                Renderer = ToolStripManager.CreateRenderer(value);
            }
        }
    }

    public event EventHandler? RendererChanged;

    private void OnRendererChanged(EventArgs e)
    {
        RendererChanged?.Invoke(this, e);
    }

    private void OnDefaultRendererChanged(object? sender, EventArgs e)
    {
        if (_state[stateUseDefaultRenderer])
        {
            OnRendererChanged(e);
        }
    }

    private void OnControlDisposed(object? sender, EventArgs e)
    {
        if (_state[stateAttachedRendererChanged])
        {
            ToolStripManager.RendererChanged -= new EventHandler(OnDefaultRendererChanged);
            _state[stateAttachedRendererChanged] = false;
        }
    }

    private void OnControlVisibleChanged(object? sender, EventArgs e)
    {
        if (sender is Control control)
        {
            if (control.Visible)
            {
                if (!_state[stateAttachedRendererChanged])
                {
                    ToolStripManager.RendererChanged += new EventHandler(OnDefaultRendererChanged);
                    _state[stateAttachedRendererChanged] = true;
                }
            }
            else
            {
                if (_state[stateAttachedRendererChanged])
                {
                    ToolStripManager.RendererChanged -= new EventHandler(OnDefaultRendererChanged);
                    _state[stateAttachedRendererChanged] = false;
                }
            }
        }
    }

    public bool ShouldSerializeRenderMode()
    {
        // We should NEVER serialize custom.
        return RenderMode != _defaultRenderMode && RenderMode != ToolStripRenderMode.Custom;
    }

    public void ResetRenderMode()
    {
        RenderMode = _defaultRenderMode;
    }
}