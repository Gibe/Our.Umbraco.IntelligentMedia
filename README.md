# Our.Umbraco.IntelligentMedia

[![Build status](https://dev.azure.com/gibeteam/Our.Umbraco.IntelligentMedia/_apis/build/status/Our.Umbraco.IntelligentMedia-build)](https://dev.azure.com/gibeteam/Our.Umbraco.IntelligentMedia/_build/latest?definitionId=1)

Adds support for adding properties relevant to images based derived using machine learning. Whenever a media item is saved it will have properties set on the media items if they are present on the media type.

## Installation
To install via nuget:

Either 
```Install-Package Our.Umbraco.IntelligentMedia.Google``` 
For Google Vision API or
```Install-Package Our.Umbraco.IntelligentMedia.Azure```
for Azure Vision API, or both to combine data from both

## Web.config Configuration
You'll need to configure AppSettings:

If you want to override the media name with one from the Vision API:
```
<add key="IntelligentMedia:OverwriteName" value="true" />
```

For Google you'll need the following:
```
<add key="IntelligentMedia:Google:ApiKey" value="" />
```
You'll need to insert your Google Vision API key

For Azure these:
```
<add key="IntelligentMedia:Azure:SubscriptionKey" value="" />
<add key="IntelligentMedia:Azure:Region" value="westeurope" />
```
You'll need to insert your Azure Vision Subscription key and region

## Umbraco Configuration
For the data to be populated on the media you'll need to add properties to the media types.

On the image media type:
Add a new tab called "AI"
Add to that tab the following properties:

| Name               | Alias              | Editor         |
| ------------------ | ------------------ | -------------- |
| Tags               | imTags             | Tags (CSV)     |
| Description        | imDescription      | Textarea       |
| Categories         | imCategories       | Tags (CSV)     |
| Number of Faces    | imNumberOfFaces    | Numeric        |
| Primary Colour     | imPrimaryColour    | Textstring     |
| Background Colour  | imBackgroundColour | Textstring     |
| Populated          | imPopulated        | Checkbox       |


