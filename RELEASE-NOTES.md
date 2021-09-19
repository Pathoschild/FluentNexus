# Release notes
## Upcoming release
* Changed mod file size fields to `long` to support larger files (thanks to Mgamerz!).

## 1.0.4
Released 12 July 2021.

* Added the new `Archived` file category.

## 1.0.3
Released 11 July 2021.

* Added an `UnmappedFields` property to all data models. This contains any fields returned by the Nexus API which don't match a known property.
* Added support for newer API fields:
  * `Mod.AllowRating` (whether the current user is allowed to endorse this mod);
  * `Mod.EndorsementCount` (the number of endorsements given by users for this mod);
  * `ModFile.SizeInBytes` (the downloaded file size in bytes);
  * `ModFile.SizeInKilobytes` (equivalent to the previous `ModFile.Size`, which still exists).

## 1.0.2
Released 30 April 2021.

* Added mod file `Description` field.
* Updated to FluentHttpClient 4.1.

## 1.0.1
Released 13 May 2020.

* Updated to FluentHttpClient 4.0.

## 1.0
Released 06 March 2020 (first beta on 03 March 2019).

* Initial release, with support for all current Nexus API endpoints.
* Added rate limit tracking and handling.
* Added support for fetching & parsing mod file content previews.
* Added support for synchronous UI contexts.
