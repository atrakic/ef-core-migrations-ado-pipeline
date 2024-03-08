SHELL:=/bin/bash
MAKEFLAGS += --silent

SQL_CONNECTION = "Server=localhost;UID=sa;PWD=${MSSQL_SA_PASSWORD};trusted_connection=false;Persist Security Info=False;Encrypt=False;Connection Timeout=3;"

BASEDIR = $(shell git rev-parse --show-toplevel)
MNAME = $(shell git rev-parse --abbrev-ref HEAD)-$(shell date +%s)

DB ?= db
PROJECT ?= demo


.PHONY: all clean db run migrations healthcheck

all: clean db run

db:
	DOCKER_BUILDKIT=1 docker-compose up --no-color --remove-orphans -d
	docker-compose ps -a
	while ! \
		[[ "$$(docker inspect --format "{{json .State.Health }}" $(DB) | jq -r ".Status")" == "healthy" ]];\
		do \
		echo "waiting $(DB) ..."; \
		sleep 1; \
		done

run:
	SQL_CONNECTION=${SQL_CONNECTION} dotnet run --project ${BASEDIR}/src/${PROJECT}.csproj

migrations:
	SQL_CONNECTION=${SQL_CONNECTION} dotnet ef migrations add ${MNAME} --project ${BASEDIR}/src/${PROJECT}.csproj
	SQL_CONNECTION=${SQL_CONNECTION} dotnet ef database update --project ${BASEDIR}/src/${PROJECT}.csproj
	SQL_CONNECTION=${SQL_CONNECTION} dotnet ef migrations list --project ${BASEDIR}/src/${PROJECT}.csproj

healthcheck:
	docker inspect $(DB) --format "{{ (index (.State.Health.Log) 0).Output }}"

clean:
	dotnet clean
	docker-compose down --remove-orphans -v --rmi local

-include .env
