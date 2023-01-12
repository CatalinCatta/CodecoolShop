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
    id       int IDENTITY (1,1) PRIMARY KEY,
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
    id         int IDENTITY (1,1) PRIMARY KEY,
    user_id    int not null,
    product_id int not null,
    quantity   int      default 1,
    date       datetime default current_timestamp
);

INSERT INTO category
VALUES ('Hardware',
        'A tablet computer, commonly shortened to tablet, is a thin, flat mobile computer with a touchscreen display.');
INSERT INTO category
VALUES ('Game',
        'An activity usually involving skill, knowledge, or chance.');
INSERT INTO category
VALUES ('Motorcycle',
        'A two-wheeled vehicle with an engine.');

INSERT INTO suppliers
VALUES ('Amazon', 'Digital content and services');
INSERT INTO suppliers
VALUES ('Lenovo', 'Computers');
INSERT INTO suppliers
VALUES ('Logitech', 'Designs, manufactures, and markets cordless products');
INSERT INTO suppliers
VALUES ('Rockstar', 'Still waiting for gta6.');
INSERT INTO suppliers
VALUES ('Valve', 'Threeless.');
INSERT INTO suppliers
VALUES ('Endnight Games Ltd', '');
INSERT INTO suppliers
VALUES ('Global Sales Distribution', '');
INSERT INTO suppliers
VALUES ('Sport Bike Conection', '');

INSERT INTO products
VALUES ('Amazon Fire', 49.9, 'USD',
        'Fantastic price. Large content ecosystem. Good parental controls. Helpful technical support.',
        1, 1);
INSERT INTO products
VALUES ('Lenovo IdeaPad Miix 700', 479.0, 'USD',
        'Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.',
        1, 2);
INSERT INTO products
VALUES ('Amazon Fire HD 8', 89.0, 'USD', 'Amazon''s latest Fire HD 8 tablet is a great value for media consumption.',
        1, 1);
INSERT INTO products
VALUES ('Keyboard and Mouse Combo', 17.99, 'USD',
        'This wireless keyboard and mouse combo features 8 multimedia hotkeys for instant access to the Internet, email, play/pause, and volume so you can easily check out your favorite sites.',
        1, 3);
INSERT INTO products
VALUES ('Webcam', 68.99, 'USD', 'Full HD 1080p video calling and recording at 30 fps.',
        1, 3);
INSERT INTO products
VALUES ('Grand Theft Auto V Premium Online Edition PC', 12.91, 'USD', 'Experience Rockstar Games critically acclaimed open world game, Grand Theft Auto V.',
        2, 4);
INSERT INTO products
VALUES ('Red Dead Redemption 2 (Ultimate Edition)', 30.57, 'USD', 'Red Dead Redemption 2 is an action-adventure game it was released on Xbox One and PlayStation 4, and since 2019 it is also available on the PC.',
        2, 4);
INSERT INTO products
VALUES ('Bully Scholarship Edition', 4.19, 'USD', 'Bully: Scholarship Edition takes place at the fictional New England boarding school, Bullworth Academy and tells the story of mischievous 15-year-old Jimmy Hopkins as he goes through the hilarity and awkwardness of adolescence.',
        2, 4);
INSERT INTO products
VALUES ('Grand Theft Auto IV Complete Edition', 8.21, 'USD', 'This standalone retail title spans three distinct stories, interwoven to create one of the most unique and engaging single-player experiences of this generation.',
        2, 4);
INSERT INTO products
VALUES ('Left 4 Dead 2', 10.57, 'USD', 'Set in the zombie apocalypse, Left 4 Dead 2 (L4D2) is the highly anticipated sequel to the award-winning Left 4 Dead, the #1 co-op game of 2008.',
        2, 5);
INSERT INTO products
VALUES ('Left 4 Dead', 10.57, 'USD', 'From Valve (the creators of Counter-Strike, Half-Life and more) comes Left 4 Dead, a co-op action horror game for the PC and Xbox 360 that casts up to four players in an epic struggle for survival against swarming zombie hordes and terrifying mutant monsters.',
        2, 5);
INSERT INTO products
VALUES ('Portal', 10.57, 'USD', N'Portal™ is a new single player game from Valve. Set in the mysterious Aperture Science Laboratories, Portal has been called one of the most innovative new games on the horizon and will offer gamers hours of unique gameplay.',
        2, 5);
INSERT INTO products
VALUES ('Portal 2', 10.57, 'USD', 'Portal 2 draws from the award-winning formula of innovative gameplay, story, and music that earned the original Portal over 70 industry accolades and created a cult following.',
        2, 5);
INSERT INTO products
VALUES ('The Forest', 18.21, 'USD', 'As the lone survivor of a passenger jet crash, you find yourself in a mysterious forest battling to stay alive against a society of cannibalistic mutants.',
        2, 6);
INSERT INTO products
VALUES ('Sons Of The Forest', 29.99, 'USD', 'Sent to find a missing billionaire on a remote island, you find yourself in a cannibal-infested hellscape. Craft, build, and struggle to survive, alone or with friends, in this terrifying new open-world survival horror simulator.',
        2, 6);
INSERT INTO products
VALUES ('Neutron', 65000.00, 'USD', 'This bike is in like-new condition having been displayed numerous times at events and shows but not driven on the street or titled.',
        3, 7);
INSERT INTO products
VALUES ( N'Kawasaki Ninja H2®R', 57500.00, 'USD', N'The development of the Ninja H2®R motorcycle goes beyond the boundaries of any other Kawasaki motorcycle.',
        3, 8);

INSERT INTO users_log
VALUES ('user_test', 'user_test@example.com',
        'ESE5x6tkvE3P7T/rvD/nEx2lCRplbNJwTCSgD7/kZR19KX+wN3icg2g61LAucrnPZTyai6lVa83bhv2AHuvKv7RBP0LII9+ZiBxJEYUO81wl8nmMcFICPcIsDLDK0yBbV/gfRmDM5rwNdZmXbf7jorsWV3eaciCmFUD22l2bF3bNMqkDYEhxlsdV7ttNrnsvyJs2mjYuhfSa5p9jkOwvyDj7/1SA+CWHd/r5rtFwxR3CtlpfJE2iKW9sITA7mMhT82422d3Wb19+sRFFPCH5IJ0NRW9nYfFmZgh95OczfE4+YQ9TrXZ7zMR7+WYEG2tTovj861N9+34Kl9rHFNeCKg==',
        'NMtW8w=='); -- password = test

INSERT INTO admins
VALUES ('admin_test', 'admin_test@example.com',
        'ESE5x6tkvE3P7T/rvD/nEx2lCRplbNJwTCSgD7/kZR19KX+wN3icg2g61LAucrnPZTyai6lVa83bhv2AHuvKv7RBP0LII9+ZiBxJEYUO81wl8nmMcFICPcIsDLDK0yBbV/gfRmDM5rwNdZmXbf7jorsWV3eaciCmFUD22l2bF3bNMqkDYEhxlsdV7ttNrnsvyJs2mjYuhfSa5p9jkOwvyDj7/1SA+CWHd/r5rtFwxR3CtlpfJE2iKW9sITA7mMhT82422d3Wb19+sRFFPCH5IJ0NRW9nYfFmZgh95OczfE4+YQ9TrXZ7zMR7+WYEG2tTovj861N9+34Kl9rHFNeCKg==',
        'NMtW8w=='); -- password = test

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