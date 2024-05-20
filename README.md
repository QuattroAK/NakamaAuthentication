# Player Authentication using Nakama

## Description

This project is a player authentication system using the Nakama game server, with authentication tokens saved to a JSON file. The MVVM architectural pattern is used to organize the code.

## Authentication Methods

- Device
- Email
- Google Play Games

## Technology Stack

- [Nakama](https://github.com/heroiclabs/nakama-unity)
- [VContainer](https://github.com/hadashiA/VContainer)
- [UniTask](https://github.com/Cysharp/UniTask)
- [R3](https://github.com/Cysharp/R3)
- [NuGet](https://github.com/GlitchEnzo/NuGetForUnity)
- [External Dependency Manager](https://github.com/googlesamples/unity-jar-resolver)
- [Newtonsoft.Json](https://github.com/applejag/Newtonsoft.Json-for-Unity)
- [Google Play Games](https://github.com/playgameservices/play-games-plugin-for-unity)

## Note

This project can be used as a template for creating your own authentication system. Server settings and authentication via Google Play Games in Nakama are configured individually for each project.

## Requirements

- Unity 2020.3 or higher
- Nakama server installed and configured
- Google Play Console account for using Google Play Games

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/QuattroAK/NakamaAuthentication.git
    ```
2. Open the project in Unity.
3. Install dependencies using the [External Dependency Manager](https://github.com/googlesamples/unity-jar-resolver).
4. Configure the Nakama server and Google Play Games by following the documentation.

## Usage

1. Open the `AuthenticationScene` scene.
2. In the Unity Inspector, configure the authentication settings according to your Nakama server and Google Play Games.
3. Run the project in the editor or on a device.

## License

This project is licensed under the MIT License. For details, see the [LICENSE](LICENSE) file.

## Contact

If you have any questions or suggestions, please contact us through Issues on GitHub.

---

I hope this `README.md` file will be useful and help other developers easily understand and use your project. Good luck!
