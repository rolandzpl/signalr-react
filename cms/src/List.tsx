import * as signalR from "@microsoft/signalr";
import { useEffect, useReducer } from "react";


type WebsiteDto = {
    slug: string
};

type Action =
    | { type: 'websiteAdded', slug: string }
    | { type: 'websiteChanged', slug: string }
    | { type: 'websiteDeleted', slug: string };

function reduce(state: WebsiteDto[], action: Action): WebsiteDto[] {

    switch (action.type) {
        case "websiteChanged":
            return [...state, { slug: action.slug }]
        default:
            return [...state, { slug: action.slug }]
    }
}

const List = () => {
    const [data, dispatch] = useReducer(reduce, []);
    useEffect(
        () => {
            const connection = new signalR.HubConnectionBuilder()
                .configureLogging(signalR.LogLevel.Debug)
                .withUrl("http://localhost:5103/api/notifications", {
                    skipNegotiation: true,
                    transport: signalR.HttpTransportType.WebSockets
                })
                .build();

            connection.on("SendWebsiteChanged", (slug: string) => {
                dispatch({ type: "websiteChanged", slug: slug });
            });

            connection.start().catch((err) => document.write(err));
        }, []);
    return (<div>
        {data?.map(x => (
            <div>{x.slug}</div>
        ))}
    </div>)
}

export default List;
