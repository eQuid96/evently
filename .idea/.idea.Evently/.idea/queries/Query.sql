SELECT
    e.id AS                     Id,
    e.title AS                  Title,
    e.description AS            Description,
    e.location AS               Location,
    e.starts_at_utc AS          StartsAt,
    e.category_id AS            CategoryId,
    categories.name AS          Category,
    e.ends_at_utc AS            EndsAt,
    ticket_type.id AS           Id,
    ticket_type.event_id AS     EventId,
    ticket_type.name AS         Name,
    ticket_type.price AS        Price,
    ticket_type.quantity AS     Quantity,
    ticket_type.currency AS     Currency
FROM events.events AS e
         INNER JOIN events.categories as categories on categories.id = e.category_id
         LEFT JOIN events.ticket_types ticket_type on ticket_type.event_id = e.id
WHERE e.id = '5b7aa6ed-0b62-4359-88ba-9066f08416c0'