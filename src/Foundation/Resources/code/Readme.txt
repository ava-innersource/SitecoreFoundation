This module supports syncing the fileSystem with the global scripts/css/less/sass folders.

To enable this, you must add a handler to your web.config
Add this under modules, at the end after Sitecore OOB modules have been registered.
<add type="SF.Foundation.Resources.ResourceWatcher, SF.Foundation.Resources" name="ResourceWatcher" /> 