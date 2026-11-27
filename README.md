# SkyCD.Plugin.Feature.WebBrowser

Adds **Tools → Web Browser** to SkyCD 3.0. The window uses the Avalonia WebView runtime supplied by the SkyCD desktop host.

Install **Web Browser** from **Options → Plugins → Browse** and restart SkyCD.

To build, install the .NET 10 SDK and configure the SkyCD GitHub Packages source with a credential that has read:packages access. Run `dotnet restore --locked-mode` and then `dotnet pack -c Release --no-restore`.


The historical Visual Basic plugin and its MIT license are preserved on the [legacy-v2 branch](../../tree/legacy-v2). The current plugin is licensed under BSD-2-Clause. See [LICENSE](LICENSE).

## Releases

After a version bump merges, tag the commit with the package version prefixed by `v`. GitHub Actions builds the package, publishes it to GitHub Packages, and attaches it to the GitHub Release. The Pages workflow regenerates the marketplace manifest and the NuGet registration feed from published releases.

To mirror an existing tagged version, run **Plugin NuGet** with that version. To regenerate metadata for all published releases, run **Publish marketplace feed**.
[Marketplace manifest](https://skycd.github.io/SkyCD.Plugin.Feature.WebBrowser/plugins.json) · [NuGet service index](https://skycd.github.io/SkyCD.Plugin.Feature.WebBrowser/index.json)
