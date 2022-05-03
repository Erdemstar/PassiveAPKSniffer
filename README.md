﻿<p align="center">
	<img src="https://cdn.dribbble.com/users/27875/screenshots/7069263/media/100c4c5dd8424a854cb69944e56e05e6.jpg" width=250/>
</p>

## PassiveAPKSniffer
PassiveAPKSniffer is a Console Application written in .NET 5.0. It decompiles on the target APK using some parameters it receives from the user, and searches for sensitive data among the source files it obtains. The application working logic is as follows.

- Gets the path of Jadx, Rule and APK files from the user.
- Decompiles the target application using Jadx.
- It tries to find sensitive data by searching the contents of the source files it has obtained, depending on the regex in the Rules file that it has previously received from the user. [The larger the given regex file, the longer the search will take. 2 regex file formats have been shared with you in the related resource Resource.]
- The result obtained is written in json format.

## Details
The available arguments are as follows.
| Argument | Explain  |
|--|--|
| jadx_path   | The path to the jadx file to run.|
| download_jadx | Used for Jadx download. |
| apk_path | The APK file path to analyze. |
| rule_path | Specifies the regex rule file path to use for sensitive data search. |

## Example Usage

```csharp
PassiveAPKSniffer.exe --download_jadx

PassiveAPKSniffer.exe --jadx_path C:\Users\test\jadx\bin\jadx.bat --rule_path  C:\Users\test\jadx\Rules.json --apk_path C:\Users\test\jadx\test.apk
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)