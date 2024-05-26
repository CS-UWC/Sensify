FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /build

COPY ./Sensify.sln ./
COPY ./Sensify/*.csproj ./Sensify/

RUN dotnet restore

COPY ./Sensify ./Sensify
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine as runtime

WORKDIR /app
COPY --from=build /publish ./
EXPOSE 8080
CMD dotnet Sensify.dll
