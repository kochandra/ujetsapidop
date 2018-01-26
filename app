<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <DockerComposeProjectPath>..\docker-compose.dcproj</DockerComposeProjectPath>
  </PropertyGroup>

  <ItemGroup>
    <Folder Include="wwwroot\" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="AWSSDK.Core" Version="3.3.21.8" />
    <PackageReference Include="AWSSDK.Extensions.NETCore.Setup" Version="3.3.4" />
    <PackageReference Include="AWSSDK.RDS" Version="3.3.20" />
    <PackageReference Include="AWSSDK.S3" Version="3.3.16.2" />
    <PackageReference Include="Microsoft.AspNetCore.All" Version="2.0.5" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="2.0.1" />
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="2.0.2" />
    <PackageReference Include="Newtonsoft.Json" Version="11.0.1-beta3" />
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="2.0.1" />
  </ItemGroup>

  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0" />
  </ItemGroup>


  <ItemGroup>
    <DotNetCliToolReference Include="Amazon.ECS.Tools" Version="1.1.0" />
  </ItemGroup>

  <ItemGroup>
    <DotNetCliToolReference Include="Amazon.ElasticBeanstalk.Tools" Version="1.0.1" />
  </ItemGroup>
</Project>


