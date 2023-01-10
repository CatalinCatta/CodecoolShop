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
    id                  integer     not null,
    name                varchar(50) not null,
    default_price       decimal     not null,
    currency            varchar(50) not null,
    description         varchar(500),
    product_category_id integer     not null,
    product_supplier_id integer     not null
);

CREATE TABLE category
(
    id          integer     not null,
    name        varchar(50) not null,
    description varchar(500)
);

CREATE TABLE suppliers
(
    id          integer     not null,
    name        varchar(50) not null,
    description varchar(500)
);

CREATE TABLE users_log
(
    id       int         not null,
    username varchar(50) not null,
    password varchar(50) not null
);

CREATE TABLE users_data
(
    user_id      int not null,
    first_name   varchar(50),
    last_name    varchar(50),
    address_1    varchar(50),
    address_2    varchar(50),
    phone_number varchar(50),
    email        varchar(50),
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
    id       int         not null,
    username varchar(50) not null,
    password varchar(50) not null
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
    quantity   int default 1,
    date DATE default current_timestamp
);

INSERT INTO products
VALUES (1, 'Amazon Fire', 49.9, 'USD',
        'Fantastic price. Large content ecosystem. Good parental controls. Helpful technical support.', 1, 1);
INSERT INTO products
VALUES (2, 'Lenovo IdeaPad Miix 700', 479.0, 'USD',
        'Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.', 1, 2);
INSERT INTO products
VALUES (3, 'Amazon Fire HD 8', 89.0, 'USD', 'Amazon''s latest Fire HD 8 tablet is a great value for media consumption.',
        1, 1);

INSERT INTO category
VALUES (1, 'Hardware',
        'A tablet computer, commonly shortened to tablet, is a thin, flat mobile computer with a touchscreen display.');

INSERT INTO suppliers
VALUES (1, 'Amazon', 'Digital content and services');
INSERT INTO suppliers
VALUES (2, 'Lenovo', 'Computers');

GO


ALTER TABLE products
    ADD PRIMARY KEY (id);
ALTER TABLE category
    ADD PRIMARY KEY (id);
ALTER TABLE suppliers
    ADD PRIMARY KEY (id);
ALTER TABLE users_log
    ADD PRIMARY KEY (id);
ALTER TABLE admins
    ADD PRIMARY KEY (id);

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