This project covers storing job and trigger data in the database. Quartz supports this out-of-the-box.

This [documentation page](https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/job-stores.html#ramjobstore)
will get you started on what you need to know to start working with "**Job Stores**".

You will also need to manually specify the database schema for Quartz. Use [this](https://github.com/quartznet/quartznet/tree/main/database/tables)
page to find the SQL script suitable to your database provider.