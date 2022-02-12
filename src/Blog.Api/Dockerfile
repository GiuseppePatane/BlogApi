FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 8090

ENV ASPNETCORE_URLS=http://+:8090


FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["src/Blog.Api/Blog.Api.csproj", "src/Blog.Api/"]
RUN dotnet restore "src/Blog.Api/Blog.Api.csproj"
COPY . .
WORKDIR "/src/src/Blog.Api"
RUN dotnet build "Blog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.Api.dll"]
