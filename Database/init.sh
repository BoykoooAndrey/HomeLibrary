#!/bin/bash
set -e

SQLCMD="/opt/mssql-tools/bin/sqlcmd"

echo "Waiting for SQL Server..."

for i in {1..30}
do
    if "$SQLCMD" \
        -S mssql \
        -U sa \
        -P "$MSSQL_SA_PASSWORD" \
        -Q "SELECT 1" > /dev/null 2>&1
    then
        echo "SQL Server is ready."
        break
    fi

    if [ "$i" -eq 30 ]; then
        echo "SQL Server did not become ready in time."
        exit 1
    fi

    sleep 2
done

for file in /scripts/*.sql
do
    echo "Executing $file..."

    "$SQLCMD" \
        -S mssql \
        -U sa \
        -P "$MSSQL_SA_PASSWORD" \
        -b \
        -i "$file"
done

echo "Database initialization completed."