CREATE PROCEDURE CrearUsuario
    @EmailNormalizado nvarchar(30),
    @HashContrasena nvarchar(200),
    @UsuarioID int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- DECLARE @UsuarioID INT;

    -- Insertar el nuevo usuario
    INSERT INTO Usuario (EmailNormalizado, HashContrasena, Estado)
    VALUES (@EmailNormalizado, @HashContrasena, 1); -- Estado 1 para indicar que el usuario está activo

    -- Obtener el UsuarioID del nuevo usuario
    SET @UsuarioID = SCOPE_IDENTITY();

    -- Insertar la descripción vacía para el usuario recién creado
    INSERT INTO DescripcionUsuario (UsuarioID, Descripcion)
    VALUES (@UsuarioID, ''); -- Asignamos una cadena vacía por defecto

    -- Devolver el ID del nuevo usuario
    SELECT @UsuarioID AS UsuarioID;
END;

CREATE PROCEDURE EliminarTecnologiaUsada
    @TecnologiaID INT,
    @ProyectoID INT,
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        DELETE FROM TecnologiaUsada
        WHERE TecnologiaID = @TecnologiaID 
          AND ProyectoID = @ProyectoID 
          AND UsuarioID = @UsuarioID;

        -- Confirmar la transacción
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Si hay error, revertir cambios
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;