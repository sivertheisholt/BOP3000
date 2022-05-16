export interface ActivityLog{
    date: Date;
    username: string;
    appUserId: number;
    identifier: string;
    outputText: string;
    lobbyId?: number;
    gameName?: string;
    gameId?: number;
    headerImage?: string;
    profilePicture?: string;
    memberFollowedId?: number;
    memberFollowedUsername? : string;
}