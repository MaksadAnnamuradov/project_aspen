#!/bin/bash
# docker exec -it gnome_warfare_dev_db psql -U Gnome

background=''

while getopts "d" OPTION; do
  case "$OPTION" in
    d) background='-d' ;;
  esac
done


docker rm -f aspen_dev_db;
docker run --name aspen_dev_db $background \
  -p 5432:5432 \
  -v $(pwd)/../pgsql/00-tables.sql:/docker-entrypoint-initdb.d/0.sql \
  -v $(pwd)/../pgsql/10-seed.sql:/docker-entrypoint-initdb.d/1.sql \
  -e POSTGRES_USER=Aspen \
  -e POSTGRES_PASSWORD=Aspen \
  -e POSTGRES_DB=Aspen postgres
