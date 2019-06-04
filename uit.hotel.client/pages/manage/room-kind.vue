<template>
    <div @contextmenu.prevent="tableContext">
        <block-flex->
            <b-button
                class="m-2"
                variant="white"
                @click="$refs.room_kind_add.open()"
            >
                <icon- class="mr-1" i="plus" />
                <span>Thêm loại phòng mới</span>
            </b-button>
            <b-button
                class="m-2 ml-auto"
                variant="white"
                @click="showInactive = !showInactive"
            >
                <icon- :i="showInactive ? 'eye' : 'eye-off'" class="mx-1" />
                <span>
                    {{
                        `Đang ${
                            showInactive ? 'hiện' : 'ẩn'
                        } loại phòng đã bị vô hiệu hóa`
                    }}
                </span>
            </b-button>
        </block-flex->
        <query-
            :query="getRoomKinds"
            class="row"
            child-class="col m-2 p-0 bg-white rounded shadow-sm overflow-auto"
        >
            <template slot-scope="{ data: { roomKinds } }">
                <b-table
                    :items="roomKindsFilter(roomKinds)"
                    :fields="[
                        {
                            key: 'index',
                            label: '#',
                            class: 'table-cell-id text-center',
                            sortable: true,
                        },
                        {
                            key: 'name',
                            label: 'Tên loại phòng',
                            tdClass: (value, key, row) => {
                                if (!row.isActive)
                                    return 'table-cell-disable w-100';
                                return 'w-100';
                            },
                            sortable: true,
                        },
                        {
                            key: 'rooms',
                            label: 'Số phòng',
                            tdClass: 'text-center',
                            sortable: true,
                        },
                        {
                            key: 'numberOfBeds',
                            label: 'Số giường',
                            tdClass: 'text-right',
                        },
                        {
                            key: 'amountOfPeople',
                            label: 'Số người tối đa',
                            tdClass: 'text-right',
                        },
                    ]"
                    class="table-style"
                    @row-clicked="
                        (roomKind, $index, $event) => {
                            $event.stopPropagation();
                            $refs.context_room_kind.open(
                                currentEvent || $event,
                                {
                                    roomKind,
                                },
                            );
                            currentEvent = null;
                        }
                    "
                >
                    <template slot="index" slot-scope="data">
                        {{ data.index + 1 }}
                    </template>
                    <template slot="rooms" slot-scope="{ value }">
                        {{ value.length }} phòng
                    </template>
                </b-table>
                <div
                    v-if="roomKindsFilter(roomKinds).length === 0"
                    class="table-after"
                >
                    Không tìm thấy loại phòng nào
                </div>
            </template>
        </query->
        <context-manage-room-kind- ref="context_room_kind" :refs="$refs" />
        <popup-room-kind-add- ref="room_kind_add" />
        <popup-room-kind-update- ref="room_kind_update" />
        <popup-rate-add- ref="rate_add" />
    </div>
</template>
<script lang="ts">
import { Component, mixins } from 'nuxt-property-decorator';
import { getRoomKinds } from '~/graphql/documents';
import { DataMixin } from '~/components/mixins';
import { GetRoomKinds } from '~/graphql/types';

@Component({
    name: 'floor-room-',
})
export default class extends mixins(DataMixin({ getRoomKinds })) {
    head() {
        return {
            title: 'Quản lý loại phòng',
        };
    }

    roomKindsFilter(
        roomKinds: GetRoomKinds.RoomKinds[],
    ): GetRoomKinds.RoomKinds[] {
        if (this.showInactive) return roomKinds;
        return roomKinds.filter(rk => rk.isActive);
    }

    tableContext(event: MouseEvent) {
        const tr = (event.target as HTMLElement).closest('tr');
        if (tr !== null) {
            this.currentEvent = event;
            tr.click();
        }
    }

    currentEvent: MouseEvent | null = null;

    showInactive: boolean = false;
}
</script>