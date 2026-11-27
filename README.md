# SkyCD.Plugin.Feature.WebBrowser

Adds **Tools → Web Browser** to SkyCD 3.0. The window uses the Avalonia WebView runtime supplied by the SkyCD desktop host.

Install **Web Browser** from **Options → Plugins → Browse** and restart SkyCD.

To build, install the .NET 10 SDK and configure the SkyCD GitHub Packages source with a credential that has read:packages access. Run `dotnet restore --locked-mode` and then `dotnet pack -c Release --no-restore`.

The marketplace reads `plugins.json` and the anonymous NuGet v3 package feed under `feed/`. For each release, build a new package version, place the lowercase package file under `feed/packages/<id>/<version>/`, and update both the version index and publisher manifest. Keep previously published versions available.

The historical Visual Basic plugin and its MIT license are preserved on the [legacy-v2 branch](../../tree/legacy-v2). The current plugin is licensed under BSD-2-Clause. See [LICENSE](LICENSE).

After the feed update merges, tag that commit with the package version prefixed by `v`. GitHub Actions verifies the build, publishes the same package to GitHub Packages, and attaches it to a GitHub release. To mirror an existing tagged version, run the **Plugin NuGet** workflow with that version.
