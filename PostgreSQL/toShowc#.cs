                        try
                        {
                            conn.Open();
                            sql = @"select * from Show_productranges";
                            cmd = new NpgsqlCommand(sql, conn);

                            dt = new DataTable();

                            dt.Load(await cmd.ExecuteReaderAsync());
                            dataGridView1.DataSource = null; //очистка таблицы
                            dataGridView1.DataSource = dt;
                            updator_continue.Invoke();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }

                        