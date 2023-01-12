DROP TABLE IF EXISTS cart;
DROP TABLE IF EXISTS order_history;
DROP TABLE IF EXISTS users_data;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS category;
DROP TABLE IF EXISTS suppliers;
DROP TABLE IF EXISTS users_log;
DROP TABLE IF EXISTS admins;

CREATE TABLE products
(
    id                  integer IDENTITY (1,1) PRIMARY KEY,
    name                varchar(50) not null,
    default_price       decimal     not null,
    currency            varchar(50) not null,
    description         varchar(500),
    product_category_id integer     not null,
    product_supplier_id integer     not null
);

CREATE TABLE category
(
    id          integer IDENTITY (1,1) PRIMARY KEY,
    name        varchar(50) not null,
    description varchar(500)
);

CREATE TABLE suppliers
(
    id          integer IDENTITY (1,1) PRIMARY KEY,
    name        varchar(50) not null,
    description varchar(500)
);

CREATE TABLE users_log
(
    id       int IDENTITY (1,1) PRIMARY KEY,
    username varchar(50)  not null,
    email    varchar(50)  not null,
    password varchar(500) not null,
    salt     varchar(50)  not null
);

CREATE TABLE users_data
(
    user_id      int not null,
    first_name   varchar(50),
    last_name    varchar(50),
    address_1    varchar(50),
    address_2    varchar(50),
    phone_number varchar(50),
    city         varchar(50),
    country      varchar(50),
    zip_code     varchar(50),
    card_holder  varchar(50),
    card_number  varchar(50),
    expiry_month varchar(50),
    expiry_year  varchar(50),
    cvv          varchar(50)
);

CREATE TABLE admins
(
    id       int IDENTITY (2,1) PRIMARY KEY,
    username varchar(50)  not null,
    email    varchar(50)  not null,
    password varchar(500) not null,
    salt     varchar(50)  not null
);

CREATE TABLE cart
(
    user_id    int not null,
    product_id int not null,
    quantity   int default 1
);

CREATE TABLE order_history
(
    user_id    int not null,
    product_id int not null,
    quantity   int  default 1,
    date       DATE default current_timestamp
);

INSERT INTO products
VALUES ('Amazon Fire', 49.9, 'USD',
        'Fantastic price. Large content ecosystem. Good parental controls. Helpful technical support.', 1, 1);
INSERT INTO products
VALUES ('Lenovo IdeaPad Miix 700', 479.0, 'USD',
        'Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.', 1, 2);
INSERT INTO products
VALUES ('Amazon Fire HD 8', 89.0, 'USD', 'Amazon''s latest Fire HD 8 tablet is a great value for media consumption.',
        1, 1);

INSERT INTO category
VALUES ('Hardware',
        'A tablet computer, commonly shortened to tablet, is a thin, flat mobile computer with a touchscreen display.');

INSERT INTO suppliers
VALUES ('Amazon', 'Digital content and services');
INSERT INTO suppliers
VALUES ('Lenovo', 'Computers');

INSERT INTO users_log
VALUES ('user_test', 'user_test@example.com',
        'ESE5x6tkvE3P7T/rvD/nEx2lCRplbNJwTCSgD7/kZR19KX+wN3icg2g61LAucrnPZTyai6lVa83bhv2AHuvKv7RBP0LII9+ZiBxJEYUO81wl8nmMcFICPcIsDLDK0yBbV/gfRmDM5rwNdZmXbf7jorsWV3eaciCmFUD22l2bF3bNMqkDYEhxlsdV7ttNrnsvyJs2mjYuhfSa5p9jkOwvyDj7/1SA+CWHd/r5rtFwxR3CtlpfJE2iKW9sITA7mMhT82422d3Wb19+sRFFPCH5IJ0NRW9nYfFmZgh95OczfE4+YQ9TrXZ7zMR7+WYEG2tTovj861N9+34Kl9rHFNeCKg==',
        'NMtW8w=='); -- pass = test

INSERT INTO admins
VALUES ('admin_test', 'admin_test@example.com',
        'ESE5x6tkvE3P7T/rvD/nEx2lCRplbNJwTCSgD7/kZR19KX+wN3icg2g61LAucrnPZTyai6lVa83bhv2AHuvKv7RBP0LII9+ZiBxJEYUO81wl8nmMcFICPcIsDLDK0yBbV/gfRmDM5rwNdZmXbf7jorsWV3eaciCmFUD22l2bF3bNMqkDYEhxlsdV7ttNrnsvyJs2mjYuhfSa5p9jkOwvyDj7/1SA+CWHd/r5rtFwxR3CtlpfJE2iKW9sITA7mMhT82422d3Wb19+sRFFPCH5IJ0NRW9nYfFmZgh95OczfE4+YQ9TrXZ7zMR7+WYEG2tTovj861N9+34Kl9rHFNeCKg==',
        'NMtW8w=='); -- pass = test

GO

ALTER TABLE products
    ADD CONSTRAINT product_category_fk
        FOREIGN KEY (product_category_id)
            REFERENCES category (id);

ALTER TABLE products
    ADD CONSTRAINT product_supplier_fk
        FOREIGN KEY (product_supplier_id)
            REFERENCES suppliers (id);

ALTER TABLE users_data
    ADD CONSTRAINT user_data_fk
        FOREIGN KEY (user_id)
            REFERENCES users_log (id);

ALTER TABLE cart
    ADD CONSTRAINT cart_user_fk
        FOREIGN KEY (user_id)
            REFERENCES users_log (id);

ALTER TABLE cart
    ADD CONSTRAINT product_fk
        FOREIGN KEY (product_id)
            REFERENCES products (id);

ALTER TABLE order_history
    ADD CONSTRAINT order_history_user_fk
        FOREIGN KEY (user_id)
            REFERENCES users_log (id);

ALTER TABLE order_history
    ADD CONSTRAINT order_history_product_fk
        FOREIGN KEY (product_id)
            REFERENCES products (id);