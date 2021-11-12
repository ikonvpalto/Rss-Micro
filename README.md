# СПП часть 1, лабораторная работа 1

## Задание

Вариант 8

Разработать программу, которая загружает данные из RSS-источников (в соответствии с заданным графиком), фильтрует их по
заданным критериям и рассылает на заданные почтовые адреса. Загрузка данных, их фильтрация и рассылка должна выполняться
параллельно. График загрузки представляет собой задаваемый временной интервал, через значение которого программное
средство должно произвести опрос RSS источника. График загрузки для каждого RSS источника может иметь свое значение.
Рассылка данных также происходит через свое значение графика загрузки. В программном средстве необходимо предусмотреть
механизм, посредством которого на заданный почтовый адрес отправлялись бы только новые, только что появившееся (по
сравнению предыдущей отправкой) новости.

Модифицировать программу, полученную в работе No 5, чтобы она представляла собой множество взаимодействующих
Web-сервисов: сервис загрузки, сервис фильтрации, сервис рассылки и сервис управления.

## RSS

Поддерживаю только формат rss 2.0

[Источник новостей 1](https://auto.onliner.by/feed), [Источник новостей 2](https://onliner.by/feed)

Пример документа в формате rss 2.0:
```xml

<rss version="2.0">
    <channel>
        <title>Onliner</title>
        <link>http://www.onliner.by/</link>
        <description>Onliner</description>
        <pubDate>Fri, 22 Oct 2021 20:29:16 +0300</pubDate>
        <generator>Onliner</generator>
        <language>ru</language>
        <image>
            <url>http://content.onliner.by/img/rss/logo.png</url>
            <width>144</width>
            <height>32</height>
            <title>Onliner</title>
            <link>http://www.onliner.by/</link>
        </image>
        <atom:link href="http://www.onliner.by/feed" rel="self" type="application/rss+xml"/>
        <item>
            <title>Хакеры взломали соцсеть Трампа до ее запуска</title>
            <link>
                https://tech.onliner.by/2021/10/22/xakery-vzlomali-socset-trampa
            </link>
            <pubDate>Fri, 22 Oct 2021 20:29:16 +0300</pubDate>
            <dc:creator>Onliner | Технологии</dc:creator>
            <category>Технологии</category>
            <guid isPermaLink="false">
                https://tech.onliner.by/2021/10/22/xakery-vzlomali-socset-trampa
            </guid>
            <description>
                <p>
                    <a href="https://tech.onliner.by/2021/10/22/xakery-vzlomali-socset-trampa">
                        <img src="https://content.onliner.by/news/thumbnail/eca89725d71a318a3ec238107100eba0.jpeg"
                             alt=""/>
                    </a>
                </p>
                <p>Злоумышленники, которые утверждают, что связаны с группой Anonymous, через несколько часов после
                    анонса запуска платформы TRUTH Social создали аккаунт от имени Трампа и разместили там нецензурные
                    записи в адрес основателя Twitter, <a
                            href="https://www.rbc.ru/technology_and_media/22/10/2021/6172ce409a794747d695e84a"
                            target="_blank">пишет
                    </a> РБК.
                </p>
                <p>
                    <a href="https://tech.onliner.by/2021/10/22/xakery-vzlomali-socset-trampa">Читать далее…</a>
                </p>
            </description>
            <media:thumbnail url="https://content.onliner.by/news/thumbnail/eca89725d71a318a3ec238107100eba0.jpeg"/>
        </item>
        <item>
            <title>
                IT-услуги, работы или имущественные права? Разбираемся с проектом Налогового кодекса
            </title>
            <link>
                https://tech.onliner.by/2021/10/22/razbiraemsya-s-proektom-nalogovogo-kodeksa
            </link>
            <pubDate>Fri, 22 Oct 2021 18:34:11 +0300</pubDate>
            <dc:creator>Onliner | Технологии</dc:creator>
            <category>Технологии</category>
            <guid isPermaLink="false">
                https://tech.onliner.by/2021/10/22/razbiraemsya-s-proektom-nalogovogo-kodeksa
            </guid>
            <description>
                <p>
                    <a href="https://tech.onliner.by/2021/10/22/razbiraemsya-s-proektom-nalogovogo-kodeksa">
                        <img src="https://content.onliner.by/news/thumbnail/3b256c6a4d9f6612772bff8a190b1d0b.jpeg"
                             alt=""/>
                    </a>
                </p>
                <p>Новый Налоговый кодекс пока еще не принят и существует только в проекте, однако его активно
                    обсуждают. Возникают вопросы о том, чем именно «занимаются» индивидуальные предприниматели:
                    оказывают услуги или выполняют работы, а от этого может зависеть возможность применения ИП УСН в
                    2022 году (пока не все ИП будут лишены такого права). Сегодня этот аспект в проекте НК раскрыт
                    недостаточно, что может вылиться в недопонимание в будущем.
                </p>
                <p>
                    <a href="https://tech.onliner.by/2021/10/22/razbiraemsya-s-proektom-nalogovogo-kodeksa">Читать
                        далее…
                    </a>
                </p>
            </description>
            <media:thumbnail url="https://content.onliner.by/news/thumbnail/3b256c6a4d9f6612772bff8a190b1d0b.jpeg"/>
        </item>
    </channel>
</rss>
```

## Запуск

Для запуска рекомендую установить Docker, Windows Terminal и запустить два скрипта: `run-env.ps1` и `run-app.ps1`
Почта будет приходить на тестовый smtp-сервер, ее можно посмотреть по адресу `localhost:8025`
Также можно посмотреть расписание отправок сообщений по адресу `localhost:5008/hangfire`, там же можно и запустить рассылку немедленно
