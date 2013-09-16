CREATE TABLE [Uri] (
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, -- Id de fila en la tabla
	[AbsoluteUri] VARCHAR(1000) UNIQUE NOT NULL, -- La Uri en sí
	[Parent] INT NULL FOREIGN KEY REFERENCES [Uri]([Id]), -- El id de la semilla de la que se obtuvo. Null para las semillas
	[Cache] CHAR(11) NULL, -- Nombre del doc en local
	[Status] BIT NULL -- null=sin procesar, 1=ok, 0=fail
);
CREATE NONCLUSTERED INDEX [Uri_Status] ON [Uri] (
	[Status]
);