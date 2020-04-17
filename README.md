# WWW.DE-BAAY.NL-Online-Comic-Reader
GitHub Repo for Online Comic Reader from VS2019

Based on the Cmic engine by Adam Hathcock: https://archive.codeplex.com/?p=comictool and the SharpCompress library by him,
I created a Comic Web API which serves a list of Comics in JSON which in turn can be called per page to be viewed.

I still want to implement Swagger to further document the API and extend the functionalities.

The API is used by a self written Web Reader (ComicReaderWebCore and a Xamarin App for Android (https://github.com/scdebaay/ComicReaderApp).

The implementation needs an SQL database and comes with a .NET Core service to scan a folder containing comics and add them to the database.

Earlier implementations of both the API and the Web Reader can be found in this Repo.
