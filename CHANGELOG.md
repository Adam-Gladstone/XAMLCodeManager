# Issues

# Wishlist
- separate the comment from the code in the XAML file; display them separately in the detail view and don't copy the comment --> perhaps make this optional
- ensure the formatting is maintained
- add a refresh button (if you add new templates, repopulate the list)


# Changes
## 17/06/2024
- Initial design and requirements
- Update the application title, icon, assets ...
- Add settings: directory for templates
- Populate list view from directory contents

## 28/06/2024
- added support for master/details view. Instead of using the CommunityToolkit generated code we use the framework described in: https://xamlbrewer.wordpress.com/2022/02/07/building-a-master-detail-page-with-winui-3-and-mvvm/
This has some additional features: filter/search on the list; app command bar; listview items with icon(s).
- initial check in.

## 14/08/2024
- allow dragging (unnamed) item into the XAML editor

## 18/11/2024
- Update all projects to .NET8.0
- Issue with Windows RIDs. Error: "error NETSDK1083: The specified RuntimeIdentifier 'win10-arm64' is not recognized" 
  Update RIDs to not specify win10 (https://learn.microsoft.com/en-gb/dotnet/core/rid-catalog#windows-rids).
  Update the PublishProfiles under the project Properties.
