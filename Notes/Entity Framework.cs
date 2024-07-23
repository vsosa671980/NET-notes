// Endpoint para obtener una entidad por su clave primaria
[HttpGet("{id}")]
public async Task<ActionResult<Entity>> GetEntityById(int id)
{
    var entity = await context.Entities.FindAsync(id);
    if (entity == null)
    {
        return NotFound(); // Devuelve 404 si no se encuentra la entidad
    }
    return Ok(entity); // Devuelve 200 OK con la entidad
}

// Endpoint para obtener el primer elemento que cumple una condición
[HttpGet("search/{id}")]
public async Task<ActionResult<Entity>> GetFirstOrDefaultEntity(int id)
{
    var entity = await context.Entities.FirstOrDefaultAsync(e => e.Id == id);
    if (entity == null)
    {
        return NotFound(); // Devuelve 404 si no se encuentra la entidad
    }
    return Ok(entity); // Devuelve 200 OK con la entidad
}

// Endpoint para filtrar entidades por varios criterios
[HttpGet("filter")]
public async Task<ActionResult<IEnumerable<Entity>>> GetFilteredEntities([FromQuery] string property1, [FromQuery] string property2)
{
    var entities = await context.Entities
                                .Where(e => e.Property1 == property1 && e.Property2 == property2)
                                .ToListAsync();
    return Ok(entities); // Devuelve 200 OK con la lista de entidades filtradas
}

// Endpoint para agregar una nueva entidad
[HttpPost]
public async Task<ActionResult<Entity>> AddEntity([FromBody] Entity newEntity)
{
    if (newEntity == null)
    {
        return BadRequest(); // Devuelve 400 si el cuerpo de la solicitud es nulo
    }

    context.Entities.Add(newEntity);
    await context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetEntityById), new { id = newEntity.Id }, newEntity); // Devuelve 201 Created con la entidad recién creada
}

// Endpoint para actualizar una entidad existente
[HttpPut("{id}")]
public async Task<ActionResult> UpdateEntity(int id, [FromBody] Entity updatedEntity)
{
    if (updatedEntity == null || updatedEntity.Id != id)
    {
        return BadRequest(); // Devuelve 400 si el cuerpo de la solicitud es nulo o el ID no coincide
    }

    var entity = await context.Entities.FindAsync(id);
    if (entity == null)
    {
        return NotFound(); // Devuelve 404 si no se encuentra la entidad
    }

    entity.Property1 = updatedEntity.Property1;
    entity.Property2 = updatedEntity.Property2;
    // Actualiza otros campos aquí

    await context.SaveChangesAsync();
    return NoContent(); // Devuelve 204 No Content si la actualización es exitosa
}

// Endpoint para borrar una entidad existente
[HttpDelete("{id}")]
public async Task<ActionResult> DeleteEntity(int id)
{
    var entity = await context.Entities.FindAsync(id);
    if (entity == null)
    {
        return NotFound(); // Devuelve 404 si no se encuentra la entidad
    }

    context.Entities.Remove(entity);
    await context.SaveChangesAsync();
    return NoContent(); // Devuelve 204 No Content si la eliminación es exitosa
}

// Endpoint para borrar múltiples entidades filtradas por varios criterios
[HttpDelete("bulk")]
public async Task<ActionResult> DeleteEntities([FromQuery] string property1, [FromQuery] string property2)
{
    var entitiesToDelete = await context.Entities
                                        .Where(e => e.Property1 == property1 && e.Property2 == property2)
                                        .ToListAsync();
    if (entitiesToDelete.Count == 0)
    {
        return NotFound(); // Devuelve 404 si no se encuentran entidades para eliminar
    }

    context.Entities.RemoveRange(entitiesToDelete);
    await context.SaveChangesAsync();
    return NoContent(); // Devuelve 204 No Content si la eliminación es exitosa
}
