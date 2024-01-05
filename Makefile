MAKEFLAGS += --silent

BASEDIR=$(shell git rev-parse --show-toplevel)
DB ?= db

PROJECT ?= demo
SQL_CONNECTION ?= "Server=localhost;UID=sa;PWD=${MSSQL_SA_PASSWORD};trusted_connection=false;Persist Security Info=False;Encrypt=False;"

all: clean
	DOCKER_BUILDKIT=1 docker-compose up --no-color --remove-orphans -d
	docker-compose ps -a
	while ! \
		[[ "$$(docker inspect --format "{{json .State.Health }}" $(DB) | jq -r ".Status")" == "healthy" ]];\
		do \
		echo "waiting $(DB) ..."; \
		sleep 1; \
		done
	SQL_CONNECTION=${SQL_CONNECTION} dotnet ef migrations add InitialCreate --project ${BASEDIR}/src/${PROJECT}.csproj 
	SQL_CONNECTION=${SQL_CONNECTION} dotnet ef database update --project ${BASEDIR}/src/${PROJECT}.csproj
	SQL_CONNECTION=${SQL_CONNECTION} dotnet run --project ${BASEDIR}/src/${PROJECT}.csproj

healthcheck:
	docker inspect $(DB) --format "{{ (index (.State.Health.Log) 0).Output }}"

clean:
	dotnet clean
	docker-compose down --remove-orphans -v --rmi local

-include .env
