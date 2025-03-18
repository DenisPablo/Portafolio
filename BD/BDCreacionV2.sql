-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- Portafolio.dbo.Usuario definition

-- Drop table

-- DROP TABLE Portafolio.dbo.Usuario;

CREATE TABLE Portafolio.dbo.Usuario (
	UsuarioID int IDENTITY(1,1) NOT NULL,
	EmailNormalizado nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	HashContrasena nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Estado bit NOT NULL,
	CONSTRAINT PK__Usuario__2B3DE798C22AACEB PRIMARY KEY (UsuarioID),
	CONSTRAINT UC_NombreUsuario UNIQUE (EmailNormalizado)
);


-- Portafolio.dbo.Categoria definition

-- Drop table

-- DROP TABLE Portafolio.dbo.Categoria;

CREATE TABLE Portafolio.dbo.Categoria (
	CategoriaID int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UsuarioID int NOT NULL,
	Estado bit NOT NULL,
	CONSTRAINT PK__Categori__F353C1C55A9AB68E PRIMARY KEY (CategoriaID),
	CONSTRAINT UC_NombreCategoria UNIQUE (Nombre),
	CONSTRAINT FK_Usuario_Categoria FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID),
	CONSTRAINT FK__Categoria__Usuar__48CFD27E FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID)
);


-- Portafolio.dbo.DescripcionUsuario definition

-- Drop table

-- DROP TABLE Portafolio.dbo.DescripcionUsuario;

CREATE TABLE Portafolio.dbo.DescripcionUsuario (
	DescripcionUsuarioID int IDENTITY(1,1) NOT NULL,
	UsuarioID int NOT NULL,
	Descripcion nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__Descripc__03F625920FF52D46 PRIMARY KEY (DescripcionUsuarioID),
	CONSTRAINT UQ__Descripc__2B3DE7993ED0CC08 UNIQUE (UsuarioID),
	CONSTRAINT FK__Descripci__Usuar__4AB81AF0 FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID)
);


-- Portafolio.dbo.Proyecto definition

-- Drop table

-- DROP TABLE Portafolio.dbo.Proyecto;

CREATE TABLE Portafolio.dbo.Proyecto (
	ProyectoID int IDENTITY(1,1) NOT NULL,
	Titulo nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Descripcion nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UsuarioID int NOT NULL,
	Estado bit NOT NULL,
	FechaPubli date NULL,
	CategoriaID int NULL,
	CONSTRAINT PK__Proyecto__CF241D45FD25CBE1 PRIMARY KEY (ProyectoID),
	CONSTRAINT UC_Titulo UNIQUE (Titulo),
	CONSTRAINT FK_Proyecto_Categoria FOREIGN KEY (CategoriaID) REFERENCES Portafolio.dbo.Categoria(CategoriaID),
	CONSTRAINT FK_Usuario_Proyecto FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID),
	CONSTRAINT FK__Proyecto__Usuari__4F7CD00D FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID)
);


-- Portafolio.dbo.Tecnologia definition

-- Drop table

-- DROP TABLE Portafolio.dbo.Tecnologia;

CREATE TABLE Portafolio.dbo.Tecnologia (
	TecnologiaID int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UsuarioID int NOT NULL,
	Estado bit NOT NULL,
	URLIcon varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__Tecnolog__669E2C1742137F5B PRIMARY KEY (TecnologiaID),
	CONSTRAINT UC_Nombre UNIQUE (Nombre),
	CONSTRAINT FK_Usuario_Tecnologia FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID),
	CONSTRAINT FK__Tecnologi__Usuar__52593CB8 FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID)
);


-- Portafolio.dbo.TecnologiaUsada definition

-- Drop table

-- DROP TABLE Portafolio.dbo.TecnologiaUsada;

CREATE TABLE Portafolio.dbo.TecnologiaUsada (
	TecnologiaUsadaID int IDENTITY(1,1) NOT NULL,
	ProyectoID int NOT NULL,
	TecnologiaID int NOT NULL,
	UsuarioID int NOT NULL,
	CONSTRAINT PK__Tecnolog__B9F3D148FF25C051 PRIMARY KEY (TecnologiaUsadaID),
	CONSTRAINT FK_TecnologiaUsada_Proyecto FOREIGN KEY (ProyectoID) REFERENCES Portafolio.dbo.Proyecto(ProyectoID),
	CONSTRAINT FK_TecnologiaUsada_Tecnologia FOREIGN KEY (TecnologiaID) REFERENCES Portafolio.dbo.Tecnologia(TecnologiaID),
	CONSTRAINT FK_TecnologiaUsada_Usuario FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID),
	CONSTRAINT FK__Tecnologi__Proye__5441852A FOREIGN KEY (ProyectoID) REFERENCES Portafolio.dbo.Proyecto(ProyectoID),
	CONSTRAINT FK__Tecnologi__Tecno__5535A963 FOREIGN KEY (TecnologiaID) REFERENCES Portafolio.dbo.Tecnologia(TecnologiaID),
	CONSTRAINT FK__Tecnologi__Usuar__5629CD9C FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID)
);


-- Portafolio.dbo.ImagenProyecto definition

-- Drop table

-- DROP TABLE Portafolio.dbo.ImagenProyecto;

CREATE TABLE Portafolio.dbo.ImagenProyecto (
	ImagenID int IDENTITY(1,1) NOT NULL,
	ProyectoID int NOT NULL,
	URL nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Orden int NOT NULL,
	UsuarioID int NOT NULL,
	PublicID nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Estado bit NULL,
	CONSTRAINT PK__ImagenPr__0C7D20D7B46AAEE6 PRIMARY KEY (ImagenID),
	CONSTRAINT UC_Url UNIQUE (URL),
	CONSTRAINT FK_ImagenProyecto_Proyecto FOREIGN KEY (ProyectoID) REFERENCES Portafolio.dbo.Proyecto(ProyectoID),
	CONSTRAINT FK_ImagenProyecto_Usuario FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID),
	CONSTRAINT FK_Imagenes_Proyectos FOREIGN KEY (UsuarioID) REFERENCES Portafolio.dbo.Usuario(UsuarioID),
	CONSTRAINT FK__ImagenPro__Proye__4BAC3F29 FOREIGN KEY (ProyectoID) REFERENCES Portafolio.dbo.Proyecto(ProyectoID)
);