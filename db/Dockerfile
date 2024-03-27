FROM postgres:16.2
ENV POSTGRES_DB service-center
ENV POSTGRES_USER postgres
ENV POSTGRES_PASSWORD root
COPY backup.sql /docker-entrypoint-initdb.d/