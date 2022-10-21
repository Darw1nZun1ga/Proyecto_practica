using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class UsuarioDatos
    {
        public async Task<bool> LoginAsync(string codigo, string clave) //Valida si usuario ingreso correctamente su usuario y contrasenia
        {
            bool valido = false; //Falso por que solo devolvera true si usuario y contrasenia son correctos
            try // captura errores pero da posibilidad de enviar alertas y que programa no se cierre bruscamente
            {
                string sql = "SELECT 1 FROM usuario WHERE Codigo=@Codigo AND Clave=@Clave;";//Permite consultar en base de datos si usuario y calve estan en DB
                //select devuelve 1 si en valido from que es de la tabla usuario mientras codigo y clave sean correctos

                using (MySqlConnection _conexion = new MySqlConnection(CadenaConexion.Cadena))//Using permite abrir y cerrar conexion automaticamente
                {
                    await _conexion.OpenAsync();// Para abrir cadena de conexion
                    using (MySqlCommand comando = new MySqlCommand(sql, _conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;//CommandType de tipo texto
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 20).Value = codigo;
                        comando.Parameters.Add("@Clave", MySqlDbType.VarChar, 120).Value = clave;
                        valido = Convert.ToBoolean(await comando.ExecuteScalarAsync());//Ejecuta comandos y devuelve un valor escalar y se lo pasamos a variable valido y lo convertimos a booleano
                    }
                }
            }
            catch (Exception ex) // traen codigos para alertar que tipo de error se ha desencadenado ej si se cae coneccion o servidor etc
            {

            }
            return valido;//devolvemos la variable valido a la funcion LoginAsync
        }

        public async Task<DataTable> DevolverListaAsync()//Permite colsultar o devolver todos los datos y usuarios de la tabla usuario
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM usuario;";//Sentencia (SELECT*) para consultar todos los datos de una tabla * significa todos y FROM de que tabla

                using (MySqlConnection _conexion = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _conexion.OpenAsync();
                    using (MySqlCommand comando = new MySqlCommand(sql, _conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;
                        MySqlDataReader dr = (MySqlDataReader)await comando.ExecuteReaderAsync();//Mysqldatareader captura o lee todos los datos que ejecuta el comando
                        dt.Load(dr);//Para pasar los datos del datareader
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dt;//Devuelve el DataTable
        }

        public async Task<bool> InsertarAsync(Usuario usuario)//Para insertar un nuevo usuario
        {
            bool inserto = false;//Detecta si se sinserto correctamente o hubo errores
            try
            {
                string sql = "INSERT INTO usuario VALUES (@Codigo, @Nombre, @Clave, @Correo, @Rol, @EstaActivo);";//INSERT INTO para agregra y luego nombre de la tabla a la que se insertara VALUES para insertar los valores

                using (MySqlConnection _conexion = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _conexion.OpenAsync();
                    using (MySqlCommand comando = new MySqlCommand(sql, _conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 20).Value = usuario.Codigo;
                        comando.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = usuario.Nombre;
                        comando.Parameters.Add("@Clave", MySqlDbType.VarChar, 120).Value = usuario.Clave;
                        comando.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = usuario.Correo;
                        comando.Parameters.Add("@Rol", MySqlDbType.VarChar, 20).Value = usuario.Rol;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = usuario.EstadoActivo;

                        await comando.ExecuteNonQueryAsync();//Va ejecutar pero no devolver ningun valor por que solo se esta registrando
                        inserto = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return inserto;//Devuelve el valor 
        }

        public async Task<bool> ActualizarAsync(Usuario usuario) //Para actualizar la lista de usuarios
        {
            bool actualizo = false;
            try
            {
                string sql = "UPDATE usuario SET Nombre=@Nombre, Clave=@Clave, Correo=@Correo, Rol=@Rol, EstaActivo=@EstaActivo WHERE Codigo=@Codigo;";//UPDATE para actualizar Nombre de la table y SET para decir que datos se actualizaran

                using (MySqlConnection _conexion = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _conexion.OpenAsync();
                    using (MySqlCommand comando = new MySqlCommand(sql, _conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 20).Value = usuario.Codigo;
                        comando.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = usuario.Nombre;
                        comando.Parameters.Add("@Clave", MySqlDbType.VarChar, 120).Value = usuario.Clave;
                        comando.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = usuario.Correo;
                        comando.Parameters.Add("@Rol", MySqlDbType.VarChar, 20).Value = usuario.Rol;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = usuario.EstadoActivo;

                        await comando.ExecuteNonQueryAsync();
                        actualizo = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return actualizo;
        }

        public async Task<bool> EliminarAsync(string codigo)//Para eliminar un usuario
        {
            bool elimino = false;
            try
            {
                string sql = "DELETE FROM usuario WHERE Codigo = @Codigo;";// DELETE FROM para saber la tabla y WHERE para indicar de que parametro

                using (MySqlConnection _conexion = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _conexion.OpenAsync();
                    using (MySqlCommand comando = new MySqlCommand(sql, _conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 20).Value = codigo;
                        await comando.ExecuteNonQueryAsync();
                        elimino = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return elimino;
        }
    }
}
