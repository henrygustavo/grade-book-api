FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["GradeBook.Api/GradeBook.Api.csproj", "GradeBook.Api/"]
COPY ["GradeBook.Infrastructure/GradeBook.Infrastructure.csproj", "GradeBook.Infrastructure/"]
COPY ["GradeBook.Domain/GradeBook.Domain.csproj", "GradeBook.Domain/"]
COPY ["GradeBook.Application/GradeBook.Application.csproj", "GradeBook.Application/"]
RUN dotnet restore "GradeBook.Api/GradeBook.Api.csproj"
COPY . .
WORKDIR "/src/GradeBook.Api"
RUN dotnet build "GradeBook.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GradeBook.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GradeBook.Api.dll"]