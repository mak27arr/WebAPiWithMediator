#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER $POSTGRES__PRODUCT__USER WITH PASSWORD '$POSTGRES__PRODUCT__PASSWORD';
    CREATE DATABASE $POSTGRES__PRODUCT__DB;
    GRANT ALL PRIVILEGES ON DATABASE $POSTGRES__PRODUCT__DB TO $POSTGRES__PRODUCT__USER;
    ALTER DATABASE $POSTGRES__PRODUCT__DB OWNER TO $POSTGRES__PRODUCT__USER;

    CREATE USER $POSTGRES__INVENTORYSERVICE__USER WITH PASSWORD '$POSTGRES__INVENTORYSERVICE__PASSWORD';
    CREATE DATABASE $POSTGRES__INVENTORYSERVICE__DB;
    GRANT ALL PRIVILEGES ON DATABASE $POSTGRES__INVENTORYSERVICE__DB TO $POSTGRES__INVENTORYSERVICE__USER;
    ALTER DATABASE $POSTGRES__INVENTORYSERVICE__DB OWNER TO $POSTGRES__INVENTORYSERVICE__USER;
EOSQL

