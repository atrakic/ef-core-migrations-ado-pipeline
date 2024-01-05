# ef-core-migrations-ado-pipeline

[![ci](https://github.com/atrakic/def-core-migrations-ado-pipeline/actions/workflows/ci.yml/badge.svg)](https://github.com/atrakic/ef-core-migrations-ado-pipeline/actions/workflows/ci.yml)
[![license](https://img.shields.io/github/license/atrakic/ef-core-migrations-ado-pipeline.svg)](https://github.com/atrakic/ef-core-migrations-ado-pipeline/blob/main/LICENSE)


> Example how to run DotNet EF core migrations using docker and [Azure DevOps pipelines](.azdo/build.yml).

### Developer setup

```sh
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.0
```

### See also
- [EFCore docs](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
- [EfCore Samples](https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core)
