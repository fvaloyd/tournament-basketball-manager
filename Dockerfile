FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-image
WORKDIR /home/app

COPY ./*.sln ./

COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

COPY tests/UnitTests/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p tests/UnitTests/${file%.*}/ && mv $file tests/UnitTests/${file%.*}/; done

COPY ./Nuget.config ./
RUN dotnet restore --configfile ./Nuget.config

COPY . .

RUN dotnet test 

RUN dotnet publish ./src/Presentation.API/Presentation.API.csproj -c release -o /publish/ --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /publish
COPY --from=build-image /publish .
ENTRYPOINT ["dotnet", "Presentation.API.dll"]