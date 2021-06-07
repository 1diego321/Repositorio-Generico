
<h1 align="center">Repositorio generico para EntityFrameworkCore5</h1>

• Repositorio generico para EFCore5. <br>
• Se puede compilar y agregar las referencias DLL al proyecto o bien se pueden agregar las clases al proyecto y hacer los cambios deseados. <br>
• Unicamente se debe indicar la clase entidad en el Generic del GenericRepository y enviar una instancia de DbContext en el constructor. <br>
• Cuenta con la abstracción para implementar la inyección de dependencias. <br>
• <b>IMPORTANTE:</b> Para guardar los cambios realizados en la base de datos se debe llamar al metodo SaveChanges del DbContext, esto lo hice
  así por si se desea combinar con una <a href="https://www.c-sharpcorner.com/UploadFile/b1df45/unit-of-work-in-repository-pattern/#:~:text=Unit%20of%20Work%20is%20the,update%2Fdelete%20and%20so%20on.">
  Unidad de Trabajo (Patrón de diseño)</a>. 


