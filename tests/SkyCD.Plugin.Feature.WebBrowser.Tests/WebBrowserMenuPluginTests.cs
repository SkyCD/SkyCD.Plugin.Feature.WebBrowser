using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkyCD.SDK.Capabilities.Menu;
using SkyCD.Testing;
using Xunit;

namespace SkyCD.Plugin.Feature.WebBrowser.Tests;

public sealed class WebBrowserMenuPluginTests
{
    [Fact]
    public void PluginAssembly_DeclaresWebBrowserCapabilityAndHostRange()
    {
        var plugin = PluginAssemblyMetadata.Inspect(typeof(WebBrowserMenuPlugin).Assembly);

        Assert.Equal("SkyCD.Plugin.WebBrowser", plugin.Id);
        Assert.Equal(3, plugin.AssemblyVersion.Major);
        Assert.Equal("3.0.0", plugin.MinHostVersion);
        Assert.Equal(3, Version.Parse(plugin.MaxHostVersion!).Major);
        Assert.Contains(typeof(WebBrowserMenuPlugin), plugin.CapabilityTypes);
    }

    [Fact]
    public async Task ExecuteMenuCommandAsync_NoContextUrl_UsesAboutBlank()
    {
        Uri? launchedUrl = null;
        var plugin = new WebBrowserMenuPlugin((uri, _) =>
        {
            launchedUrl = uri;
            return Task.FromResult(true);
        });

        await plugin.ExecuteMenuCommandAsync("webbrowser.open", new MenuCommandContext());

        Assert.Equal(new Uri("about:blank"), launchedUrl);
    }

    [Fact]
    public async Task ExecuteMenuCommandAsync_InvalidUrl_NotifiesHostAndSkipsLaunch()
    {
        var hostApi = new RecordingHostCommandApi();
        var launchCalled = false;
        var plugin = new WebBrowserMenuPlugin((_, _) =>
        {
            launchCalled = true;
            return Task.FromResult(true);
        });

        await plugin.ExecuteMenuCommandAsync("webbrowser.open", new MenuCommandContext
        {
            Properties = new Dictionary<string, string> { ["url"] = "javascript:alert(1)" },
            HostApi = hostApi
        });

        Assert.False(launchCalled);
        Assert.Single(hostApi.Notifications);
        Assert.Contains("Unsupported URL scheme", hostApi.Notifications[0]);
    }

    [Fact]
    public async Task ExecuteMenuCommandAsync_ValidUrl_LaunchesResolvedUrl()
    {
        Uri? launchedUrl = null;
        var hostApi = new RecordingHostCommandApi();
        var plugin = new WebBrowserMenuPlugin((uri, _) =>
        {
            launchedUrl = uri;
            return Task.FromResult(true);
        });

        await plugin.ExecuteMenuCommandAsync("webbrowser.open", new MenuCommandContext
        {
            Properties = new Dictionary<string, string> { ["url"] = "https://skycd.example/" },
            HostApi = hostApi
        });

        Assert.Equal(new Uri("https://skycd.example/"), launchedUrl);
        Assert.Empty(hostApi.Notifications);
    }

    [Fact]
    public async Task ExecuteMenuCommandAsync_LaunchFailure_NotifiesHost()
    {
        var hostApi = new RecordingHostCommandApi();
        var plugin = new WebBrowserMenuPlugin((_, _) => Task.FromResult(false));

        await plugin.ExecuteMenuCommandAsync("webbrowser.open", new MenuCommandContext
        {
            Properties = new Dictionary<string, string> { ["url"] = "https://skycd.example/" },
            HostApi = hostApi
        });

        Assert.Single(hostApi.Notifications);
        Assert.Contains("Failed to open URL", hostApi.Notifications[0]);
    }
}
