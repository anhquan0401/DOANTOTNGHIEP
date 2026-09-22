# Sử dụng base image của .NET SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file .csproj và restore các thư viện
COPY ["WebApplication1.csproj", "./"]
RUN dotnet restore "WebApplication1.csproj"

# Copy toàn bộ code và build
COPY . .
RUN dotnet publish "WebApplication1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Chuyển sang base image nhỏ gọn chỉ chứa Runtime để chạy ứng dụng (giảm dung lượng)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy file đã build từ bước trên sang
COPY --from=build /app/publish .

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
