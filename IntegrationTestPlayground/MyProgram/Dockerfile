#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyProgram/MyProgram.csproj", "MyProgram/"]
RUN dotnet restore "MyProgram/MyProgram.csproj"
COPY . .
WORKDIR "/src/MyProgram"
RUN dotnet build "MyProgram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProgram.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProgram.dll", "Server=.;Database=safechat-dev;MultipleActiveResultSets=false;User Id=sa;Password=SpikePrototype2016!;Trusted_Connection=False;TrustServerCertificate=False;Encrypt=False"]